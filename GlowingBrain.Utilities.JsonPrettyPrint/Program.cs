using System;
using System.IO;
using GlowingBrain.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GlowingBrain.Utilities.JsonPrettyPrint
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            var settings = new Settings();
            settings.Parse(args);

            if (settings.ShowHelp)
            {
                settings.WriteOptionDescriptions();
                return 0;
            }
            if (settings.ShowExampleRulesFile)
            {
                ShowExampleRulesFile();
                return 0;
            }

            TextReader reader = null;

            try
            {
                // read from file or console if file not specified
                reader = settings.InputFile == null ? Console.In : File.OpenText(settings.InputFile);
                var jsonIn = reader.ReadToEnd();

                // get input object
                var deserializedObject = JsonConvert.DeserializeObject<JToken>(jsonIn);

                // build the formatter
                PropertyFormattingRulesCollection rules = null;
                if (settings.RulesFile != null)
                {
                    var rulesDto = JsonConvert.DeserializeObject<PropertyRulesCollectionDto>(File.ReadAllText(settings.RulesFile));
                    rules = rulesDto.ToModel();
                }
                var formatter = new JsonFormatter(rules);

                // serialize the formatted json
                var jsonOut = formatter.Stringify(deserializedObject);

                // write to file or console if no file specified
                if (settings.OutputFile == null)
                {
                    Console.Out.WriteLine(jsonOut);
                }
                else
                {
                    File.WriteAllText(settings.OutputFile, jsonOut);
                }
            }
            catch (Exception ex)
            {
                // something nasty happened
                Console.Error.WriteLine("Abnormal program termination: exception encountered");
                Console.Error.WriteLine(ex);
                return 1;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                }
            }

            // all good
            return 0;
        }

        private static void ShowExampleRulesFile()
        {
            var rules = new PropertyRulesCollectionDto
            {
                new PropertyRulesDto
                {
                    PropertyPattern = "ArrayOfInts",
                    Rules = new FormattingRules
                    {
                        NewLineBeforeArrayItem = false,
                        NewLineBeforeArrayStart = false,
                        NewLineBeforeArrayEnd = false
                    }
                },
                new PropertyRulesDto
                {
                    PropertyPattern = "ChildObject",
                    Rules = new FormattingRules
                    {
                        NewLineBeforeArrayItem = false,
                        NewLineBeforeArrayStart = false,
                        NewLineBeforeArrayEnd = false,
                        IncludePropertiesWithNullValues = false,
                        NewLineBeforeProperty = false,
                        NewLineBeforeObjectEnd = false
                    }
                },
                new PropertyRulesDto
                {
                    PropertyPattern = "ChildObject.GrandChildObject",
                    Rules = new FormattingRules
                    {
                        IncludePropertiesWithNullValues = true
                    }
                }
            };

            var formatter = new JsonFormatter();
            formatter.PropertyFormattingRulesCollection.AddPatternRule(".*", new FormattingRules { IncludePropertiesWithNullValues = false });
            var jObject = JsonConvert.DeserializeObject<JToken>(JsonConvert.SerializeObject(rules));
            Console.Out.WriteLine(formatter.Stringify(jObject));
        }
    }
}