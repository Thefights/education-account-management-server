using DTOs.TopUp;
using Mappers.Base;
using Models;
using Riok.Mapperly.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace Mappers
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    
    public partial class TopupRuleMapper : ICrudMapper<TopupRule, CreateTopupRuleDTO, GetTopupRuleDTO, UpdateTopupRuleDTO>
    {
        [MapProperty(nameof(TopupRule.Conditions), nameof(GetTopupRuleDTO.Conditions))]
        public partial GetTopupRuleDTO MapToGetDTO(TopupRule model);

        public partial List<GetTopupRuleDTO> MapToGetDTOList(List<TopupRule> models);

        public partial IQueryable<GetTopupRuleDTO> ProjectToGetDTO(IQueryable<TopupRule> query);

        public TopupRule MapFromCreateDTO(CreateTopupRuleDTO createDTO) => MapToRule(createDTO);

        public void MapFromUpdateDTO(UpdateTopupRuleDTO updateDTO, TopupRule model) => MapToRule(updateDTO, model);

        public partial TopupRule MapToRule(CreateTopupRuleDTO dto);

        public partial void MapToRule(UpdateTopupRuleDTO dto, TopupRule model);

        public partial TopupRuleConditionDTO MapToConditionDTO(TopupRuleCondition model);
    }
}
