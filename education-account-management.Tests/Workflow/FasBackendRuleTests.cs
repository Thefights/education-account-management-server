using DTOs.FasSchemes;
using Enums;
using Exceptions;
using Helpers.FasSchemes;
using Services.FasSchemes;

namespace education_account_management.Tests.Workflow;

public class FasBackendRuleTests
{
    [Fact]
    public void SelectHighestMatchingTier_UsesHalfOpenRanges()
    {
        var tiers = new[]
        {
            new FasSchemeTier
            {
                Id = 1,
                TierName = "Tier 1",
                TierIncomeBasis = FasTierIncomeBasis.PerCapitaIncome,
                MinPerCapitaIncome = 0m,
                MaxPerCapitaIncome = 1000m,
                DisplayOrder = 1
            },
            new FasSchemeTier
            {
                Id = 2,
                TierName = "Tier 2",
                TierIncomeBasis = FasTierIncomeBasis.PerCapitaIncome,
                MinPerCapitaIncome = 1000m,
                MaxPerCapitaIncome = 2000m,
                DisplayOrder = 2
            },
            new FasSchemeTier
            {
                Id = 3,
                TierName = "Tier 3",
                TierIncomeBasis = FasTierIncomeBasis.PerCapitaIncome,
                MinPerCapitaIncome = 2000m,
                MaxPerCapitaIncome = null,
                DisplayOrder = 3
            }
        };

        Assert.Equal(1, FasTierMatcher.SelectHighestMatchingTier(tiers, 999.99m, 0m)!.Id);
        Assert.Equal(2, FasTierMatcher.SelectHighestMatchingTier(tiers, 1000m, 0m)!.Id);
        Assert.Equal(3, FasTierMatcher.SelectHighestMatchingTier(tiers, 5000m, 0m)!.Id);
    }

    [Fact]
    public void SelectHighestMatchingTier_ForPciOrGrossMultiMatch_UsesHighestTierDisplayOrder()
    {
        var tiers = new[]
        {
            new FasSchemeTier
            {
                Id = 1,
                TierName = "Higher",
                TierIncomeBasis = FasTierIncomeBasis.PerCapitaOrGrossHouseholdIncome,
                MinPerCapitaIncome = 0m,
                MaxPerCapitaIncome = 1000m,
                MinGrossHouseholdIncome = 0m,
                MaxGrossHouseholdIncome = 3000m,
                DisplayOrder = 1
            },
            new FasSchemeTier
            {
                Id = 2,
                TierName = "Lower",
                TierIncomeBasis = FasTierIncomeBasis.PerCapitaOrGrossHouseholdIncome,
                MinPerCapitaIncome = 1000m,
                MaxPerCapitaIncome = null,
                MinGrossHouseholdIncome = 3000m,
                MaxGrossHouseholdIncome = null,
                DisplayOrder = 2
            }
        };

        var matched = FasTierMatcher.SelectHighestMatchingTier(tiers, perCapitaIncome: 900m, grossHouseholdIncome: 5000m);

        Assert.Equal(1, matched!.Id);
    }

    [Fact]
    public void ValidateTierConfiguration_BlocksGapAndOverlap()
    {
        var gap = new List<FasSchemeTierRequestDTO>
        {
            Tier("Tier 1", 0m, 1000m, 1),
            Tier("Tier 2", 1500m, null, 2)
        };

        Assert.Throws<ValidationFailureException>(() =>
            FasTierMatcher.ValidateTierConfiguration(gap, FasSubsidyType.Percent, isPerComponent: false));

        var overlap = new List<FasSchemeTierRequestDTO>
        {
            Tier("Tier 1", 0m, 1000m, 1),
            Tier("Tier 2", 900m, null, 2)
        };

        Assert.Throws<ValidationFailureException>(() =>
            FasTierMatcher.ValidateTierConfiguration(overlap, FasSubsidyType.Percent, isPerComponent: false));
    }

    [Fact]
    public void FasConditionSemanticAnalyzer_BlocksImpossibleScenarios()
    {
        var root = new FasConditionGroupRequestDTO
        {
            LogicalOperator = TopupLogicalOperator.And,
            Conditions =
            [
                new FasConditionRequestDTO
                {
                    Field = FasConditionField.StudentAge,
                    Operator = FasConditionOperator.Equal,
                    ValueNumber = 18m
                },
                new FasConditionRequestDTO
                {
                    Field = FasConditionField.StudentAge,
                    Operator = FasConditionOperator.Equal,
                    ValueNumber = 20m,
                    DisplayOrder = 1
                }
            ]
        };

        Assert.Throws<ValidationFailureException>(() => FasConditionSemanticAnalyzer.Validate(root));
    }

    [Fact]
    public void FasChargeSubsidyCalculator_CalculatesNormalAndPerComponentAfterGst()
    {
        var normal = FasChargeSubsidyCalculator.Calculate(
            courseFee: 100m,
            miscFee: 50m,
            taxRate: 0.09m,
            grossAmount: 163.5m,
            new FasApplication
            {
                FasScheme = new FasScheme
                {
                    SubsidyType = FasSubsidyType.Percent,
                    IsPerComponent = false
                },
                ApprovedTier = new FasSchemeTier
                {
                    SubsidyValue = 50m
                }
            });

        Assert.Equal(81.75m, normal.SubsidyAmount);
        Assert.Equal(81.75m, normal.NetAmount);

        var perComponent = FasChargeSubsidyCalculator.Calculate(
            courseFee: 100m,
            miscFee: 50m,
            taxRate: 0.09m,
            grossAmount: 163.5m,
            new FasApplication
            {
                FasScheme = new FasScheme
                {
                    SubsidyType = FasSubsidyType.Percent,
                    IsPerComponent = true
                },
                ApprovedTier = new FasSchemeTier
                {
                    CourseFeeSubsidyValue = 50m,
                    MiscFeeSubsidyValue = 20m
                }
            });

        Assert.Equal(65.4m, perComponent.SubsidyAmount);
        Assert.Equal(98.1m, perComponent.NetAmount);
    }

    [Fact]
    public void FasChargeSubsidyCalculator_CapsFixedSubsidy()
    {
        var result = FasChargeSubsidyCalculator.Calculate(
            courseFee: 60m,
            miscFee: 40m,
            taxRate: 0m,
            grossAmount: 100m,
            new FasApplication
            {
                FasScheme = new FasScheme
                {
                    SubsidyType = FasSubsidyType.FixedAmount,
                    IsPerComponent = false
                },
                ApprovedTier = new FasSchemeTier
                {
                    SubsidyValue = 200m
                }
            });

        Assert.Equal(100m, result.SubsidyAmount);
        Assert.Equal(0m, result.NetAmount);
    }

    private static FasSchemeTierRequestDTO Tier(string name, decimal min, decimal? max, int displayOrder)
    {
        return new FasSchemeTierRequestDTO
        {
            TierName = name,
            TierIncomeBasis = FasTierIncomeBasis.PerCapitaIncome,
            MinPerCapitaIncome = min,
            MaxPerCapitaIncome = max,
            SubsidyValue = 10m,
            DisplayOrder = displayOrder
        };
    }
}
