using System.Text.RegularExpressions;

namespace GlowingBrain.Json
{
    /// <summary>
    /// Specifies a set of formatting rules to apply to all properties whose path
    /// matches the regular expression specified by a property pattern
    /// </summary>
    public class PropertyFormattingRules
    {
        /// <summary>
        /// Initializes an instance if the <see cref="PropertyFormattingRules"/> Type.
        /// </summary>
        /// <param name="propertyPattern">Specifies a regular expression to determine which properties the formatting rules should be applied to</param>
        /// <param name="formattingRules">The formatting rules to apply when writting the JSON data</param>
        public PropertyFormattingRules(string propertyPattern, FormattingRules formattingRules)
        {
            this.PropertyPatternRegex = new Regex(propertyPattern);
            this.FormattingRules = formattingRules;
            this.Priority = propertyPattern.Length;
        }

        /// <summary>
        /// Gets the regular expression to determine which properties the formatting rules should be applied to
        /// </summary>
        public Regex PropertyPatternRegex { get; private set; }

        /// <summary>
        /// Gets the formatting rules
        /// </summary>
        public FormattingRules FormattingRules { get; private set; }  

        /// <summary>
        /// Gets the priority of the rule
        /// </summary>
        /// <remarks>
        /// In the current implementation the priority is derived from the length of the 
        /// property pattern. Longer patterns have a larger priority than shorter patterns
        /// and will thus be applied after
        /// </remarks>
        public int Priority { get; private set; }  
    }
}