using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GlowingBrain.Json.Infrastructure;
using Newtonsoft.Json.Linq;

namespace GlowingBrain.Json
{
    /// <summary>
    /// Formatted JSON data writer that uses rules to determine how an object graph is written
    /// </summary>
    public class JsonFormatter
    {
        /// <summary>
        /// Initializes an instance of the <see cref="JsonFormatter"/> Type
        /// </summary>
        /// <param name="propertyFormattingRulesCollection"></param>
        public JsonFormatter(PropertyFormattingRulesCollection propertyFormattingRulesCollection = null)
        {
            PropertyFormattingRulesCollection = propertyFormattingRulesCollection ?? new PropertyFormattingRulesCollection();
        }

        /// <summary>
        /// Gets the property formatting rules collection which determine how output data is written
        /// </summary>
        public PropertyFormattingRulesCollection PropertyFormattingRulesCollection { get; private set; }

#if false
        /// <summary>
        /// Writes the given JSON data to the specified file
        /// </summary>
        /// <param name="value">The JSON data to be serialized</param>
        /// <param name="pathname">The pathname of the file where the JSON text is to be written</param>
        public void Write(JObject value, string pathname)
        {
            using (var writer = new StreamWriter(pathname))
            {
                Write(value, writer);
            }
        }

#endif

        /// <summary>
        /// Writes the given JSON data to the <see cref="TextWriter"/> instance
        /// </summary>
        /// <param name="value">The JSON data to be serialized</param>
        /// <param name="writer">The <see cref="TextWriter"/> where the data is to be written</param>
        public void Write(JToken value, TextWriter writer)
        {
            Write(value, new Context(writer, PropertyFormattingRulesCollection));
        }

        /// <summary>
        /// Writes the given JSON data to the resulting string
        /// </summary>
        /// <param name="value">The JSON data to be serialized</param>
        /// <returns>A string containing the JSON representation of the object</returns>
        public string Stringify(JToken value)
        {
            using (var writer = new StringWriter())
            {
                Write(value, writer);
                return writer.ToString();
            }    
        }

        private void Write(JToken value, Context context)
        {
            switch (value.Type)
            {
                case JTokenType.Object:
                    WriteObject(value, context);
                    break;
                case JTokenType.Array:
                    WriteArray(value, context);
                    break;
                case JTokenType.Boolean:
                    WriteBoolean(value, context);
                    break;
                case JTokenType.Integer:
                    WriteInteger(value, context);
                    break;
                case JTokenType.Float:
                    WriteFloat(value, context);
                    break;
                case JTokenType.String:
                    WriteString(value, context);
                    break;
                case JTokenType.Null:
                    WriteNullValue(context);
                    break;
                case JTokenType.Date:
                    WriteDate(value, context);
                    break;
            }
        }

        private static void WriteNullValue(Context context)
        {
            context.WriteNullValue();
        }

        private static void WriteBoolean(JToken value, Context context)
        {
            context.WriteBoolean((bool) value);
        }

        private static void WriteInteger(JToken value, Context context)
        {
            context.WriteInteger((int) value);
        }

        private static void WriteFloat(JToken value, Context context)
        {
            context.WriteFloat((float) value);
        }

        private static void WriteString(JToken value, Context context)
        {
            context.WriteString((string) value);
        }

        private static void WriteDate(JToken value, Context context)
        {
            context.WriteDate((DateTime)value);
        }

        private void WriteObject(JToken value, Context context)
        {
            context.WriteStartObject();

            var isFirstItem = true;
            foreach (var child in value.Children())
            {
                var property = (JProperty)child;
                if (context.ShouldWriteProperty(property))
                {
                    context.WritePropertyName(property.Name, isFirstItem);
                    context.PushProperty(property.Name);
                    Write(property.Value, context);
                    context.PopProperty();
                    isFirstItem = false;
                }
            }

            context.WriteCloseObject();
        }

        private void WriteArray(JToken value, Context context)
        {
            context.WriteStartArray();

            var isFirstItem = true;
            foreach (var child in value.Children())
            {
                context.WriteStartArrayItem(isFirstItem);
                Write(child, context);
                context.WriteEndArrayItem();
                isFirstItem = false;
            }

            context.WriteEndArray();
        }

        private class Context
        {
            private readonly List<Nesting> _nesting = new List<Nesting>();
            private readonly PropertyFormattingRulesCollection _propertyFormattingRulesCollection;
            private readonly TextWriter _writer;
            private bool _haveWrittenToStreamWriter;

            public Context(TextWriter writer, PropertyFormattingRulesCollection propertyFormattingRulesCollection)
            {
                _writer = writer;
                _propertyFormattingRulesCollection = propertyFormattingRulesCollection;
                _nesting.Add(new Nesting());
                ApplyRule();
            }
            
            public bool ShouldWriteProperty(JProperty property)
            {
                return property.Value.Type != JTokenType.Null ||
                       CurrentFormattingRules.IncludePropertiesWithNullValues.HasValueOf(true);
            }

            public void PushProperty(string propertyName)
            {
                Log.Debug("Pushing property: {0}", propertyName);

                var currentNesting = CurrentNesting;
                _nesting.Add(new Nesting(currentNesting.Level, propertyName));
                ApplyRule();
            }

            public void PopProperty()
            {
                Log.Debug("Popping property");

                _nesting.RemoveAt(_nesting.Count - 1);
                ApplyRule();
            }

            public void PushObject()
            {
                Log.Debug("Pushing object");

                var currentNesting = CurrentNesting;
                _nesting.Add(new Nesting(currentNesting.Level + 1, currentNesting.PropertyName));
            }

            public void PopObject()
            {
                Log.Debug("Popping object");

                _nesting.RemoveAt(_nesting.Count - 1);
                ApplyRule();
            }
            
            public void PushArray()
            {
                Log.Debug("Pushing array");

                var currentNesting = CurrentNesting;
                _nesting.Add(new Nesting(currentNesting.Level + 1, currentNesting.PropertyName));
            }

            public void PopArray()
            {
                Log.Debug("Popping array");

                _nesting.RemoveAt(_nesting.Count - 1);
                ApplyRule();
            }
            
            public void WriteStartObject()
            {
                PushObject();

                CurrentFormattingRules.NewLineBeforeObjectStart.WhenValueEquals(true,
                    () => NewLineAndIndent(CurrentNesting.Level));

                Write("{");
            }

            public void WriteCloseObject()
            {
                PopObject();

                CurrentFormattingRules.NewLineBeforeObjectEnd.WhenValueEquals(true,
                    () => NewLineAndIndent(CurrentNesting.Level));

                CurrentFormattingRules.NewLineBeforeObjectEnd.WhenValueEquals(false,
                    () => WriteWhitespace(CurrentFormattingRules.WhitespaceBeforeObjectEnd.GetValueOrDefault(1)));

                

                Write("}");
            }

            public void WriteStartArrayItem(bool isFirst)
            {
                if (isFirst)
                {
                    if (CurrentFormattingRules.NewLineBeforeArrayItem.HasValueOf(false))
                    {
                        WriteWhitespace(CurrentFormattingRules.WhiteSpaceBeforeFirstArrayItem.GetValueOrDefault(1));
                    }
                }
                else
                {
                    Write(",");
                    WriteWhitespace(CurrentFormattingRules.WhitespaceBeforeNonFirstArrayItem.GetValueOrDefault(1));
                }

                CurrentFormattingRules.NewLineBeforeArrayItem.WhenValueEquals(true,
                    () => NewLineAndIndent(CurrentNesting.Level));
            }

            public void WriteEndArrayItem()
            {
            }

            public void WritePropertyName(string propertyName, bool isFirst)
            {
                if (isFirst)
                {
                    if (CurrentFormattingRules.NewLineBeforeProperty.HasValueOf(false))
                    {
                        WriteWhitespace(CurrentFormattingRules.WhiteSpaceBeforeFirstProperty.GetValueOrDefault(1));
                    }
                }
                else
                {
                    Write(",");
                }

                CurrentFormattingRules.NewLineBeforeProperty.WhenValueEquals(true,
                    () => NewLineAndIndent(CurrentNesting.Level));
                Write("\"" + propertyName + "\":");
                WriteWhitespace(CurrentFormattingRules.WhitespaceAfterProperty.GetValueOrDefault(1));
            }

            public void WriteStartArray()
            {
                PushArray();

                CurrentFormattingRules.NewLineBeforeArrayStart.WhenValueEquals(true,
                    () => NewLineAndIndent(CurrentNesting.Level));

                Write("[");
            }

            public void WriteEndArray()
            {
                PopArray();

                CurrentFormattingRules.NewLineBeforeArrayEnd.WhenValueEquals(true,
                    () => NewLineAndIndent(CurrentNesting.Level));

                CurrentFormattingRules.NewLineBeforeArrayEnd.WhenValueEquals(false,
                    () => WriteWhitespace(CurrentFormattingRules.WhitespaceBeforeArrayEnd.GetValueOrDefault(1)));
                
                Write("]");
            }

            public void WriteBoolean(bool value)
            {
                Write(value ? "true" : "false");
            }

            public void WriteNullValue()
            {
                Write("null");
            }

            public void WriteFloat(float value)
            {
                Write(value);
            }

            public void WriteInteger(int value)
            {
                Write(value);
            }

            public void WriteDate(DateTime value)
            {
                Write("\"" + value.ToString("O") + "\"");
            }

            public void WriteString(string value)
            {
                if (value == null)
                {
                    Write("null");
                }
                else
                {
                    Write("\"" + value + "\"");
                }
            }

            private Nesting CurrentNesting
            {
                get { return _nesting[_nesting.Count - 1]; }
            }

            private string PropertyPathString
            {
                get
                {
                    var propertyNames = _nesting.Skip(1).Select(n => n.PropertyName).ToList();
                    return string.Join(".", propertyNames);
                }
            }

            private FormattingRules CurrentFormattingRules { get; set; }

            private void ApplyRule()
            {
                var path = PropertyPathString;
                Log.Debug("Applying rules for path: {0}", path);

                var rule = _propertyFormattingRulesCollection.GetEffectiveFormattingRules(path);
                Log.Debug(rule.ToString());

                CurrentFormattingRules = rule;
            }

            private void NewLineAndIndent(int level)
            {
                WriteLine();
                WriteWhitespace(level * CurrentFormattingRules.IndentSize.GetValueOrDefault(2));
            }

            private void WriteWhitespace(int count)
            {
                count.Times(() => Write(" "));
            }

            private void Write(int value)
            {
                _writer.Write(value);
                _haveWrittenToStreamWriter = true;
            }

            private void Write(float value)
            {
                _writer.Write(value);
                _haveWrittenToStreamWriter = true;
            }

            private void Write(string value)
            {
                _writer.Write(value);
                _haveWrittenToStreamWriter = true;
            }

            private void WriteLine(string value = null)
            {
                if (value != null)
                {
                    Write(value);
                }

                _haveWrittenToStreamWriter.WhenTrue(() => _writer.WriteLine());
                _haveWrittenToStreamWriter = true;
            }

            private class Nesting
            {
                public Nesting(int level = 0, string propertyName = "")
                {
                    Level = level;
                    PropertyName = propertyName;
                }

                public string PropertyName { get; }
                public int Level { get; }
            }
        }
    }
}