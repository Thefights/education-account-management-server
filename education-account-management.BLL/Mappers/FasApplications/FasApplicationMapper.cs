using DTOs.FasApplications;
using Riok.Mapperly.Abstractions;
using Mappers.Base;

namespace Mappers.FasApplications
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class FasApplicationMapper : IReadMapper<FasApplication, GetFasApplicationSchoolAdminDTO>
    {
        [MapProperty(nameof(FasApplication.Id), nameof(GetFasApplicationSchoolAdminDetailDTO.Id))]
        [MapProperty(nameof(FasApplication.StudentAgeSnapshot), $"{nameof(GetFasApplicationSchoolAdminDetailDTO.StudentProfile)}.{nameof(StudentProfileDTO.Age)}")]
        [MapProperty(nameof(FasApplication.GrossHouseholdIncomeSnapshot), $"{nameof(GetFasApplicationSchoolAdminDetailDTO.StudentProfile)}.{nameof(StudentProfileDTO.GrossHouseholdIncome)}")]
        [MapProperty(nameof(FasApplication.HouseholdMemberCountSnapshot), $"{nameof(GetFasApplicationSchoolAdminDetailDTO.StudentProfile)}.{nameof(StudentProfileDTO.HouseholdMembers)}")]
        [MapProperty(nameof(FasApplication.PerCapitaIncomeSnapshot), $"{nameof(GetFasApplicationSchoolAdminDetailDTO.StudentProfile)}.{nameof(StudentProfileDTO.PerCapitaIncome)}")]
        [MapperIgnoreTarget(nameof(GetFasApplicationSchoolAdminDetailDTO.Scheme))]
        [MapperIgnoreTarget(nameof(GetFasApplicationSchoolAdminDetailDTO.SystemSuggestedTier))]
        [MapperIgnoreTarget(nameof(GetFasApplicationSchoolAdminDetailDTO.ApprovedTier))]
        [MapperIgnoreTarget(nameof(GetFasApplicationSchoolAdminDetailDTO.TierOverrideHistories))]
        [MapperIgnoreTarget(nameof(GetFasApplicationSchoolAdminDetailDTO.Status))]
        private partial GetFasApplicationSchoolAdminDetailDTO MapToDetailDTOInternal(FasApplication model);

        public GetFasApplicationSchoolAdminDetailDTO MapToDetailDTO(FasApplication model)
        {
            var target = MapToDetailDTOInternal(model);

            target.Status = model.Status.ToString();
            target.StudentProfile.StudentNationality = model.StudentNationalitySnapshot.ToString();
            target.StudentProfile.GuardianNationality = model.GuardianNationalitySnapshot.ToString();
            target.Scheme = new SchemeDetailsDTO
            {
                Id = model.FasSchemeId,
                SchemeName = model.FasScheme.SchemeName,
                Tiers = model.FasScheme.Tiers.Select(MapTierToDTO).ToList(),
                RequiredDocuments = model.FasScheme.RequiredDocuments.Select(rd =>
                {
                    var attachedDoc = model.Documents.FirstOrDefault(d => d.FasSchemeRequiredDocumentId == rd.Id);
                    return new ApplicationDocumentDTO
                    {
                        RequiredDocumentId = rd.Id,
                        ApplicationDocumentId = attachedDoc?.Id,
                        DocumentName = rd.DocumentName,
                        FileName = attachedDoc?.FileName,
                        FileKey = attachedDoc?.FileKey
                    };
                }).ToList()
            };

            if (model.RecommendedTier != null)
            {
                target.SystemSuggestedTier = new SystemSuggestedTierDTO
                {
                    Id = model.RecommendedTier.Id,
                    TierName = model.RecommendedTier.TierName,
                    Reason = model.RecommendationReason ?? $"Matches {model.RecommendedTier.TierName} bracket."
                };
            }

            if (model.ApprovedTier != null)
            {
                target.ApprovedTier = new ApprovedTierDTO
                {
                    Id = model.ApprovedTier.Id,
                    TierName = model.ApprovedTier.TierName
                };
            }

            target.TierOverrideHistories = model.TierOverrideHistories
                .OrderByDescending(history => history.ModifiedAt)
                .Select(history => new TierOverrideHistoryDTO
                {
                    Id = history.Id,
                    OldTierId = history.OldTierId,
                    OldTierName = history.OldTier?.TierName,
                    NewTierId = history.NewTierId,
                    NewTierName = history.NewTier?.TierName ?? string.Empty,
                    RecommendationReason = model.RecommendationReason ?? string.Empty,
                    Reason = history.Reason,
                    ModifiedByUserId = history.ModifiedByUserId,
                    ModifiedByName = history.ModifiedByUser?.AdminProfile != null
                        ? history.ModifiedByUser.AdminProfile.FullName
                        : null,
                    ModifiedAt = history.ModifiedAt
                })
                .ToList();

            target.AdditionalAnswers = model.AdditionalQuestionAnswers.Select(a => new ApplicationAdditionalAnswerDTO
            {
                Id = a.Id,
                QuestionText = a.QuestionTextSnapshot,
                AnswerText = a.AnswerText,
                IsRequired = a.IsRequiredSnapshot
            }).ToList();

            return target;
        }

        [MapProperty(nameof(FasSchemeTier.Id), nameof(TierDetailsDTO.Id))]
        [MapProperty(nameof(FasSchemeTier.TierName), nameof(TierDetailsDTO.TierName))]
        [MapProperty(nameof(FasSchemeTier.TierIncomeBasis), nameof(TierDetailsDTO.TierIncomeBasis))]
        [MapProperty(nameof(FasSchemeTier.MinPerCapitaIncome), nameof(TierDetailsDTO.MinPerCapitaIncome))]
        [MapProperty(nameof(FasSchemeTier.MaxPerCapitaIncome), nameof(TierDetailsDTO.MaxPerCapitaIncome))]
        [MapProperty(nameof(FasSchemeTier.MinGrossHouseholdIncome), nameof(TierDetailsDTO.MinGrossHouseholdIncome))]
        [MapProperty(nameof(FasSchemeTier.MaxGrossHouseholdIncome), nameof(TierDetailsDTO.MaxGrossHouseholdIncome))]
        [MapProperty(nameof(FasSchemeTier.SubsidyType), nameof(TierDetailsDTO.SubsidyType))]
        [MapProperty(nameof(FasSchemeTier.IsPerComponent), nameof(TierDetailsDTO.IsPerComponent))]
        [MapProperty(nameof(FasSchemeTier.SubsidyValue), nameof(TierDetailsDTO.SubsidyValue))]
        [MapProperty(nameof(FasSchemeTier.CourseFeeSubsidyValue), nameof(TierDetailsDTO.CourseFeeSubsidyValue))]
        [MapProperty(nameof(FasSchemeTier.MiscFeeSubsidyValue), nameof(TierDetailsDTO.MiscFeeSubsidyValue))]
        [MapperIgnoreSource(nameof(FasSchemeTier.FasScheme))]
        public partial TierDetailsDTO MapTierToDTO(FasSchemeTier tier);

        public partial GetFasApplicationSchoolAdminDTO MapToGetDTO(FasApplication model);
        public partial List<GetFasApplicationSchoolAdminDTO> MapToGetDTOList(List<FasApplication> models);
        public IQueryable<GetFasApplicationSchoolAdminDTO> ProjectToGetDTO(IQueryable<FasApplication> query)
        {
            return query.Select(a => new GetFasApplicationSchoolAdminDTO
            {
                Id = a.Id,
                ApplicationNumber = a.ApplicationNumber,
                AccountName = a.SchoolStudent.EducationAccount.Citizen.FullName,
                AccountNumber = a.SchoolStudent.EducationAccount.AccountNumber,
                SchemeName = a.FasScheme.SchemeName,
                SubmittedAt = a.CreatedAt,
                Status = a.Status.ToString(),
                ExternalRejectionReason = a.ExternalRejectionReason,
                InternalRejectionReason = a.InternalRejectionReason
            });
        }
    }
}
