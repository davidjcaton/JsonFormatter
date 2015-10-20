using System.Collections.Generic;
using System.Linq;
using GlowingBrain.Json.Infrastructure;

namespace GlowingBrain.Json
{
    /// <summary>
    /// Manages a collection of property formatting rules
    /// </summary>
    public class PropertyFormattingRulesCollection : List<PropertyFormattingRules>
    {
        private readonly FormattingRules _defaultFormattingRules;

        /// <summary>
        /// Initializes an instance of the <see cref="PropertyFormattingRulesCollection"/> Type
        /// </summary>
        public PropertyFormattingRulesCollection() : this (FormattingRules.Default)
        {        
        }

        /// <summary>
        /// Initializes an instance of the <see cref="PropertyFormattingRulesCollection"/> Type
        /// </summary>
        /// <param name="defaultFormattingRules">Specifies the default formatting rules</param>
        public PropertyFormattingRulesCollection(FormattingRules defaultFormattingRules)
        {
            _defaultFormattingRules = defaultFormattingRules;            
        }

        /// <summary>
        /// Adds a set of formatting rules for the given property pattern
        /// </summary>
        /// <param name="propertyPattern">The property pattern</param>
        /// <param name="formattingRules">The formatting rules</param>
        public void AddPatternRule(string propertyPattern, FormattingRules formattingRules)
        {
            this.Add(new PropertyFormattingRules(propertyPattern, formattingRules));    
        }

        /// <summary>
        /// Gets the formatting rules that should be applied to the property path
        /// </summary>
        /// <param name="propertyPath">The property path</param>
        /// <returns>The formatting rules to be applied to the property path</returns>
        /// <remarks>
        /// A property path is specified as parentProperty.childProperty.grandchildProperty
        /// format (with arbitary levels of nesting). An empty path denotes the root
        /// object of a data structure.
        /// </remarks>
        public FormattingRules GetEffectiveFormattingRules(string propertyPath)
        {
            // get all formatting rules that apply to the given property name
            // ordering them by priority such that we overlay rules at higher
            // priority
            var matches = this
                .Where(r => r.PropertyPatternRegex.IsMatch(propertyPath))
                .OrderBy(r => r.Priority)
                .ToList();

            // starting with default rules overlay matching rule property values
            var result = _defaultFormattingRules.DeepClone();
            matches.Each(match => match.FormattingRules.ApplySpecifiedRulesTo(result));

            return result;
        }
    }
}