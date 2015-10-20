using System.Text;
using GlowingBrain.Json.Infrastructure;

namespace GlowingBrain.Json
{
    /// <summary>
    /// Specifies a set of formatting rules to apply when outputting JSON format data
    /// </summary>
    public class FormattingRules
    {
        static FormattingRules()
        {
            Default = new FormattingRules
            {
                NewLineBeforeArrayStart = false,
                NewLineBeforeArrayEnd = true,
                NewLineBeforeArrayItem = true,
                NewLineBeforeObjectStart = false,
                NewLineBeforeProperty = true,
                NewLineBeforeObjectEnd = true,
                IndentSize = 2,
                WhitespaceAfterProperty = 1,
                WhitespaceBeforeNonFirstArrayItem = 1,
                IncludePropertiesWithNullValues = true,
                WhiteSpaceBeforeFirstProperty = 1,
                WhiteSpaceBeforeFirstArrayItem = 1,
                WhitespaceBeforeArrayEnd = 1,
                WhitespaceBeforeObjectEnd = 1
            };
        }

        /// <summary>
        /// Gets or sets the default formatting rules
        /// </summary>
        public static FormattingRules Default { get; set; }

        /// <summary>
        /// Determines if a new line is inserted before the start of an array
        /// </summary>
        public bool? NewLineBeforeArrayStart { get; set; }

        /// <summary>
        /// Determines if a new line is inserted before the end of an array
        /// </summary>
        public bool? NewLineBeforeArrayEnd { get; set; }

        /// <summary>
        /// Determines the amount of whitespace to include before the end of an array
        /// </summary>
        /// <remarks>
        /// This property is only effective when NewLineBeforeArrayEnd is false
        /// </remarks>
        public int? WhitespaceBeforeArrayEnd { get; set; }

        /// <summary>
        /// Determines if a new line is inserted before the start of an object
        /// </summary>
        public bool? NewLineBeforeObjectStart { get; set; }

        /// <summary>
        /// Determines if a new line is inserted before the end of an object
        /// </summary>
        public bool? NewLineBeforeObjectEnd { get; set; }

        /// <summary>
        /// Determines the amount of whitespace to include before the end of an object
        /// </summary>
        /// <remarks>
        /// This property is only effective when NewLineBeforeObjectEnd is false
        /// </remarks>
        public int? WhitespaceBeforeObjectEnd { get; set; }

        /// <summary>
        /// Determines if a new line is inserted before the start of a property
        /// </summary>
        public bool? NewLineBeforeProperty { get; set; }

        /// <summary>
        /// Determines the amount of whitespace to include before the first array item
        /// </summary>
        /// <remarks>
        /// This property is only effective when NewLineBeforeProperty is false
        /// </remarks>
        public int? WhiteSpaceBeforeFirstProperty { get; set; }

        /// <summary>
        /// Determines if a new line is inserted before the start of an array item
        /// </summary>
        public bool? NewLineBeforeArrayItem { get; set; }

        /// <summary>
        /// Determines the amount of whitespace to include before the first array item
        /// </summary>
        /// <remarks>
        /// This property is only effective when NewLineBeforeArrayItem is false
        /// </remarks>
        public int? WhiteSpaceBeforeFirstArrayItem { get; set; }
        
        /// <summary>
        /// Determines the number of whitespace characters per indent level
        /// </summary>
        public int? IndentSize { get; set; }

        /// <summary>
        /// Determines the number of whitespace characters to insert after a property name
        /// </summary>
        public int? WhitespaceAfterProperty { get; set; }

        /// <summary>
        /// Determines the number of whitespace characters to insert after a comma in an array of items
        /// </summary>
        public int? WhitespaceBeforeNonFirstArrayItem { get; set; }

        /// <summary>
        /// Determines if null property values are included in the output or are suppressed
        /// </summary>
        public bool? IncludePropertiesWithNullValues { get; set; }

        /// <summary>
        /// Performs a deep clone of the instance
        /// </summary>
        /// <returns></returns>
        public FormattingRules DeepClone()
        {
            return new FormattingRules
            {
                NewLineBeforeArrayStart = this.NewLineBeforeArrayStart,
                NewLineBeforeArrayEnd = this.NewLineBeforeArrayEnd,
                NewLineBeforeObjectStart = this.NewLineBeforeObjectStart,
                NewLineBeforeObjectEnd = this.NewLineBeforeObjectEnd,
                NewLineBeforeProperty = this.NewLineBeforeProperty,
                NewLineBeforeArrayItem = this.NewLineBeforeArrayItem,
                IndentSize = this.IndentSize,
                WhitespaceAfterProperty = this.WhitespaceAfterProperty,
                WhitespaceBeforeNonFirstArrayItem = this.WhitespaceBeforeNonFirstArrayItem,
                IncludePropertiesWithNullValues = this.IncludePropertiesWithNullValues,
                WhiteSpaceBeforeFirstArrayItem = this.WhiteSpaceBeforeFirstArrayItem,
                WhiteSpaceBeforeFirstProperty = this.WhiteSpaceBeforeFirstProperty,
                WhitespaceBeforeArrayEnd = this.WhitespaceBeforeArrayEnd,
                WhitespaceBeforeObjectEnd = this.WhitespaceBeforeObjectEnd
            };
        }

        /// <summary>
        /// Applies all specified (non null) property values to the other
        /// FormattingRules instance
        /// </summary>
        /// <param name="other"></param>
        public void ApplySpecifiedRulesTo(FormattingRules other)
        {
            NewLineBeforeArrayStart.WhenHasValue(v => other.NewLineBeforeArrayStart = v);
            NewLineBeforeArrayEnd.WhenHasValue(v => other.NewLineBeforeArrayEnd = v);
            NewLineBeforeObjectStart.WhenHasValue(v => other.NewLineBeforeObjectStart = v);
            NewLineBeforeObjectEnd.WhenHasValue(v => other.NewLineBeforeObjectEnd = v);
            NewLineBeforeProperty.WhenHasValue(v => other.NewLineBeforeProperty = v);
            NewLineBeforeArrayItem.WhenHasValue(v => other.NewLineBeforeArrayItem = v);
            IndentSize.WhenHasValue(v => other.IndentSize = v);
            WhitespaceAfterProperty.WhenHasValue(v => other.WhitespaceAfterProperty = v);
            WhitespaceBeforeNonFirstArrayItem.WhenHasValue(v => other.WhitespaceBeforeNonFirstArrayItem = v);
            IncludePropertiesWithNullValues.WhenHasValue(v => other.IncludePropertiesWithNullValues = v);
            WhiteSpaceBeforeFirstArrayItem.WhenHasValue(v => other.WhiteSpaceBeforeFirstArrayItem = v);
            WhiteSpaceBeforeFirstProperty.WhenHasValue(v => other.WhiteSpaceBeforeFirstProperty = v);
            WhitespaceBeforeArrayEnd.WhenHasValue(v => other.WhitespaceBeforeArrayEnd = v);
            WhitespaceBeforeObjectEnd.WhenHasValue(v => other.WhitespaceBeforeObjectEnd = v);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("FormattingRules: [");
            sb.AppendFormat("  NewLineBeforeArrayStart:           {0},", NewLineBeforeArrayStart); sb.AppendLine();
            sb.AppendFormat("  NewLineBeforeArrayEnd:             {0},", NewLineBeforeArrayEnd); sb.AppendLine();
            sb.AppendFormat("  NewLineBeforeObjectStart:          {0},", NewLineBeforeObjectStart); sb.AppendLine();
            sb.AppendFormat("  NewLineBeforeObjectEnd:            {0},", NewLineBeforeObjectEnd); sb.AppendLine();
            sb.AppendFormat("  NewLineBeforeProperty:             {0},", NewLineBeforeProperty); sb.AppendLine();
            sb.AppendFormat("  IndentSize:                        {0},", IndentSize); sb.AppendLine();
            sb.AppendFormat("  WhitespaceAfterProperty:           {0},", WhitespaceAfterProperty); sb.AppendLine();
            sb.AppendFormat("  WhitespaceBeforeNonFirstArrayItem: {0},", WhitespaceBeforeNonFirstArrayItem); sb.AppendLine();
            sb.AppendFormat("  IncludePropertiesWithNullValues:   {0},", IncludePropertiesWithNullValues); sb.AppendLine();
            sb.AppendFormat("  WhiteSpaceBeforeFirstArrayItem:    {0},", WhiteSpaceBeforeFirstArrayItem); sb.AppendLine();
            sb.AppendFormat("  WhiteSpaceBeforeFirstProperty:     {0},", WhiteSpaceBeforeFirstProperty); sb.AppendLine();
            sb.AppendFormat("  WhitespaceBeforeArrayEnd:          {0},", WhitespaceBeforeArrayEnd); sb.AppendLine();
            sb.AppendFormat("  WhitespaceBeforeObjectEnd:         {0},", WhitespaceBeforeObjectEnd); sb.AppendLine();
            sb.AppendLine("]");
            return sb.ToString();
        }
    }
}