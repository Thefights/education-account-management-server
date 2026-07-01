using DTOs.FasSchemes;
using Enums;
using Exceptions;
using Models;

namespace Helpers.FasSchemes
{
    public static class FasTierMatcher
    {
        public static FasSchemeTier? SelectHighestMatchingTier(
            IEnumerable<FasSchemeTier> tiers,
            decimal perCapitaIncome,
            decimal grossHouseholdIncome)
        {
            return tiers
                .Where(tier => Matches(tier, perCapitaIncome, grossHouseholdIncome))
                .OrderBy(tier => tier.DisplayOrder)
                .ThenBy(tier => tier.Id)
                .FirstOrDefault();
        }

        public static bool Matches(FasSchemeTier tier, decimal perCapitaIncome, decimal grossHouseholdIncome)
        {
            return tier.TierIncomeBasis switch
            {
                FasTierIncomeBasis.PerCapitaIncome => InRange(
                    perCapitaIncome,
                    tier.MinPerCapitaIncome,
                    tier.MaxPerCapitaIncome),

                FasTierIncomeBasis.GrossHouseholdIncome => InRange(
                    grossHouseholdIncome,
                    tier.MinGrossHouseholdIncome,
                    tier.MaxGrossHouseholdIncome),

                FasTierIncomeBasis.PerCapitaOrGrossHouseholdIncome =>
                    InRange(perCapitaIncome, tier.MinPerCapitaIncome, tier.MaxPerCapitaIncome)
                    || InRange(grossHouseholdIncome, tier.MinGrossHouseholdIncome, tier.MaxGrossHouseholdIncome),

                _ => false
            };
        }

        public static string BuildRecommendationReason(FasSchemeTier tier)
        {
            return tier.TierIncomeBasis switch
            {
                FasTierIncomeBasis.PerCapitaIncome =>
                    $"Matched PCI range {FormatRange(tier.MinPerCapitaIncome, tier.MaxPerCapitaIncome)}",
                FasTierIncomeBasis.GrossHouseholdIncome =>
                    $"Matched gross income range {FormatRange(tier.MinGrossHouseholdIncome, tier.MaxGrossHouseholdIncome)}",
                FasTierIncomeBasis.PerCapitaOrGrossHouseholdIncome =>
                    $"Matched PCI range {FormatRange(tier.MinPerCapitaIncome, tier.MaxPerCapitaIncome)} OR gross income range {FormatRange(tier.MinGrossHouseholdIncome, tier.MaxGrossHouseholdIncome)}",
                _ => "Matched tier"
            };
        }

        public static void ValidateTierConfiguration(
            IReadOnlyCollection<FasSchemeTierRequestDTO> tiers,
            FasSubsidyType subsidyType,
            bool isPerComponent)
        {
            var errors = new Dictionary<string, string>();
            if (tiers.Count == 0)
            {
                errors[nameof(CreateFasSchemeDTO.Tiers)] = "At least one tier is required.";
                throw new ValidationFailureException(errors);
            }

            foreach (var tier in tiers.Select((value, index) => new { value, index }))
            {
                ValidateTier(tier.value, tier.index, subsidyType, isPerComponent, errors);
            }

            ValidateRanges(
                tiers.Where(t => t.TierIncomeBasis is FasTierIncomeBasis.PerCapitaIncome or FasTierIncomeBasis.PerCapitaOrGrossHouseholdIncome)
                    .Select(t => new RangeConfig(t.TierName, t.DisplayOrder, t.MinPerCapitaIncome, t.MaxPerCapitaIncome))
                    .ToList(),
                "PCI",
                errors);

            ValidateRanges(
                tiers.Where(t => t.TierIncomeBasis is FasTierIncomeBasis.GrossHouseholdIncome or FasTierIncomeBasis.PerCapitaOrGrossHouseholdIncome)
                    .Select(t => new RangeConfig(t.TierName, t.DisplayOrder, t.MinGrossHouseholdIncome, t.MaxGrossHouseholdIncome))
                    .ToList(),
                "gross income",
                errors);

            if (errors.Count != 0)
            {
                throw new ValidationFailureException(errors);
            }
        }

        private static void ValidateTier(
            FasSchemeTierRequestDTO tier,
            int index,
            FasSubsidyType subsidyType,
            bool isPerComponent,
            Dictionary<string, string> errors)
        {
            var prefix = $"{nameof(CreateFasSchemeDTO.Tiers)}[{index}]";
            if (tier.DisplayOrder < 0)
                errors[$"{prefix}.{nameof(tier.DisplayOrder)}"] = "Display order cannot be negative.";
            if (!Enum.IsDefined(tier.TierIncomeBasis))
                errors[$"{prefix}.{nameof(tier.TierIncomeBasis)}"] = "Tier income basis is invalid.";

            switch (tier.TierIncomeBasis)
            {
                case FasTierIncomeBasis.PerCapitaIncome:
                    RequireRange(tier.MinPerCapitaIncome, tier.MaxPerCapitaIncome, $"{prefix}.PerCapitaIncome", errors);
                    RejectRange(tier.MinGrossHouseholdIncome, tier.MaxGrossHouseholdIncome, $"{prefix}.GrossHouseholdIncome", errors);
                    break;
                case FasTierIncomeBasis.GrossHouseholdIncome:
                    RequireRange(tier.MinGrossHouseholdIncome, tier.MaxGrossHouseholdIncome, $"{prefix}.GrossHouseholdIncome", errors);
                    RejectRange(tier.MinPerCapitaIncome, tier.MaxPerCapitaIncome, $"{prefix}.PerCapitaIncome", errors);
                    break;
                case FasTierIncomeBasis.PerCapitaOrGrossHouseholdIncome:
                    RequireRange(tier.MinPerCapitaIncome, tier.MaxPerCapitaIncome, $"{prefix}.PerCapitaIncome", errors);
                    RequireRange(tier.MinGrossHouseholdIncome, tier.MaxGrossHouseholdIncome, $"{prefix}.GrossHouseholdIncome", errors);
                    break;
            }

            if (isPerComponent)
            {
                ValidateSubsidyValue(tier.CourseFeeSubsidyValue, $"{prefix}.{nameof(tier.CourseFeeSubsidyValue)}", subsidyType, errors);
                ValidateSubsidyValue(tier.MiscFeeSubsidyValue, $"{prefix}.{nameof(tier.MiscFeeSubsidyValue)}", subsidyType, errors);
                if (tier.SubsidyValue.HasValue)
                    errors[$"{prefix}.{nameof(tier.SubsidyValue)}"] = "Subsidy value must be empty when per-component subsidy is enabled.";
            }
            else
            {
                ValidateSubsidyValue(tier.SubsidyValue, $"{prefix}.{nameof(tier.SubsidyValue)}", subsidyType, errors);
                if (tier.CourseFeeSubsidyValue.HasValue || tier.MiscFeeSubsidyValue.HasValue)
                    errors[$"{prefix}.ComponentSubsidyValue"] = "Component subsidy values must be empty when per-component subsidy is disabled.";
            }
        }

        private static void RequireRange(decimal? min, decimal? max, string path, Dictionary<string, string> errors)
        {
            if (!min.HasValue)
            {
                errors[$"{path}.Min"] = "Range minimum is required.";
                return;
            }

            if (min.Value < 0)
                errors[$"{path}.Min"] = "Range minimum cannot be negative.";
            if (max.HasValue && max.Value < 0)
                errors[$"{path}.Max"] = "Range maximum cannot be negative.";
            if (max.HasValue && min.Value >= max.Value)
                errors[$"{path}.Max"] = "Range maximum must be greater than range minimum.";
        }

        private static void RejectRange(decimal? min, decimal? max, string path, Dictionary<string, string> errors)
        {
            if (min.HasValue || max.HasValue)
                errors[path] = "This range must be empty for the selected tier income basis.";
        }

        private static void ValidateSubsidyValue(
            decimal? value,
            string path,
            FasSubsidyType subsidyType,
            Dictionary<string, string> errors)
        {
            if (!value.HasValue)
            {
                errors[path] = "Subsidy value is required.";
                return;
            }

            if (subsidyType == FasSubsidyType.Percent)
            {
                if (value.Value <= 0 || value.Value > 100)
                    errors[path] = "Percent subsidy must be greater than 0 and less than or equal to 100.";
                return;
            }

            if (value.Value <= 0)
                errors[path] = "Fixed subsidy must be greater than 0.";
        }

        private static void ValidateRanges(
            List<RangeConfig> ranges,
            string label,
            Dictionary<string, string> errors)
        {
            if (ranges.Count == 0) return;

            var ordered = ranges
                .OrderBy(r => r.Min)
                .ThenBy(r => r.DisplayOrder)
                .ToList();

            if (ordered[0].Min != 0)
            {
                errors[$"{label}.RangeStart"] = $"The first {label} tier range must start at 0.";
            }

            decimal? expectedMin = null;
            var openEndedCount = 0;
            for (var index = 0; index < ordered.Count; index++)
            {
                var range = ordered[index];
                if (expectedMin.HasValue && range.Min != expectedMin.Value)
                {
                    errors[$"{label}.RangeGap"] = $"Tier '{range.TierName}' creates a gap or overlap in {label} ranges.";
                }

                if (!range.Max.HasValue)
                {
                    openEndedCount++;
                    if (index != ordered.Count - 1)
                    {
                        errors[$"{label}.OpenEnded"] = $"Open-ended {label} tier '{range.TierName}' must be the final range.";
                    }
                }

                expectedMin = range.Max;
            }

            if (openEndedCount > 1)
            {
                errors[$"{label}.OpenEnded"] = $"Only one open-ended {label} tier range is allowed.";
            }

            if (openEndedCount == 0)
            {
                errors[$"{label}.RangeEnd"] = $"The final {label} tier range must be open-ended.";
            }
        }

        private static bool InRange(decimal value, decimal? min, decimal? max)
        {
            return min.HasValue
                && value >= min.Value
                && (!max.HasValue || value < max.Value);
        }

        private static string FormatRange(decimal? min, decimal? max)
        {
            return $"[{min?.ToString("0.##") ?? "-∞"}, {max?.ToString("0.##") ?? "∞"})";
        }

        private sealed record RangeConfig(string TierName, int DisplayOrder, decimal? Min, decimal? Max);
    }
}
