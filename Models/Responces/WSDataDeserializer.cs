using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using project2025.Models.Responces;

namespace project2025.Models.Responses
{
    public class WSDataDeserializer
    {
        public int type { get; set; }
        public object data { get; set; }

        public static WSDataDeserializer FromJson(string json)
        {
            var jObject = JObject.Parse(json);
            int type = jObject["type"].Value<int>();

            var result = new WSDataDeserializer { type = type };

            switch (type)
            {
                case 1:
                    result.data = jObject["data"].ToObject<LearnPost>();
                    break;
                case 2:
                    result.data = jObject["data"].ToObject<AddXO>();
                    break;
                default:
                    throw new Exception("Unknown type");
            }

            return result;
        }
    }
}
