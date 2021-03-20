

namespace Templates.Int
{
    using System;
    using Newtonsoft.Json;

    public class ReplaceMeIntIdNewtonSoftConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ReplaceMeIntId);
        }

        public override bool CanRead => true;

        public override bool CanWrite => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.ValueType == typeof(int) || reader.ValueType == typeof(long))
            {
                return ReplaceMeIntId.FromInt((int)reader.Value);
            }

            throw new JsonSerializationException($"Cannot deserialise ReplaceMeIntId from {reader.Value}");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is ReplaceMeIntId id)
            {
                writer.WriteValue(id.ToInt());
            }
        }
    }
}
