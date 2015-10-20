using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonPrettyPrint
{
    internal static class DataValidator
    {
        public static bool IsEquivalentToKnownGoodSerializer(JObject value, string valueAsJson)
        {
            // check the our json data is good by determining if it is 
            // equivalent to JSON.NET serialized data

            // re-serialized using JSON.NET
            var json1 = JsonConvert.SerializeObject(JObject.Parse(valueAsJson));

            // serialize the object directly
            var json2 = JsonConvert.SerializeObject(value);
            
            var result = string.Equals(json1, json2);
            return result;
        }
    }
}