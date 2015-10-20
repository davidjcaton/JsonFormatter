using System;
using System.IO;
using GlowingBrain.Json;
using Newtonsoft.Json.Linq;

namespace JsonPrettyPrint
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var test1 = JObject.FromObject(new
            {
                IntProperty = 12,
                StringProperty = "Hello World",
                DateProperty = DateTime.Now,
                Bool = true,
                NullValue = (object)null,
                ArrayOfInts = new int[] {1, 2, 3, 4, 5},
                ChildObject = new
                {
                    ChildInt = 50,
                    AnotherNull = (object)null,
                    GrandChildObject = new
                    {
                        ILoveNulls = (object)null
                    }
                },
                AnotherInt = 42,
                AnotherString = "Foo bar!"
            });

            var writer = new JsonFormatter();
            writer.PropertyFormattingRulesCollection.AddPatternRule(
                "ArrayOfInts",
                new FormattingRules
                {
                    NewLineBeforeArrayItem = false,
                    NewLineBeforeArrayStart = false,
                    NewLineBeforeArrayEnd = false
                });

            writer.PropertyFormattingRulesCollection.AddPatternRule(
                "ChildObject",
                new FormattingRules
                {
                    NewLineBeforeArrayItem = false,
                    NewLineBeforeArrayStart = false,
                    NewLineBeforeArrayEnd = false,
                    IncludePropertiesWithNullValues = false,
                    NewLineBeforeProperty = false,
                    NewLineBeforeObjectEnd = false
                });

            writer.PropertyFormattingRulesCollection.AddPatternRule(
                "ChildObject.GrandChildObject",
                new FormattingRules
                {
                    IncludePropertiesWithNullValues = true
                });

            File.WriteAllText("test1_pretty.json", writer.Stringify(test1));
            var isGood = DataValidator.IsEquivalentToKnownGoodSerializer(test1, writer.Stringify(test1));
        }
    }
}