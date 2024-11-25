using MongoDB.Bson;
using Newtonsoft.Json;

namespace MTOGO_API_Service.Data
{
    public class ObjectIdJsonConverter : JsonConverter<ObjectId>
    {
        public override ObjectId ReadJson(JsonReader reader, Type objectType, ObjectId existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                return ObjectId.Parse(reader.Value.ToString());
            }
            return ObjectId.Empty;
        }

        public override void WriteJson(JsonWriter writer, ObjectId value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
