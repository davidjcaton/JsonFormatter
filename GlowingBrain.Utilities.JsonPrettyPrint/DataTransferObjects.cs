using System.Collections.Generic;
using GlowingBrain.Json;

namespace GlowingBrain.Utilities.JsonPrettyPrint
{
    class PropertyRulesDto
    {
        public string PropertyPattern { get; set; }
        public FormattingRules Rules { get; set; }
    }

    class PropertyRulesCollectionDto : List<PropertyRulesDto>
    {
        public PropertyFormattingRulesCollection ToModel()
        {
            var model = new PropertyFormattingRulesCollection();
            foreach (var propertyRuleDto in this)
            {
                model.AddPatternRule(propertyRuleDto.PropertyPattern, propertyRuleDto.Rules);
            }
            return model;
        }
    }
}