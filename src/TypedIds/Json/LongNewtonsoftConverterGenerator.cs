using System;

namespace TypedIds.Converters
{
    internal class LongNewtonsoftConverterGenerator : BaseNewtonsoftConverterGenerator, IConverterGenerator
    {
        protected override string CreateSource(string typeName)
        {
            return $@"
    using System;
    using Newtonsoft.Json;

    public class {typeName}NsJsonConverter : JsonConverter
    {{
        public override bool CanConvert(Type objectType)
        {{
            return objectType == typeof({typeName});
        }}

        public override bool CanRead => true;

        public override bool CanWrite => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {{
            if (reader.ValueType == typeof(int))
            {{
                return {typeName}.FromLong((long)(int)reader.Value);
            }}

            if (reader.ValueType == typeof(long))
            {{
                return {typeName}.FromLong((long)reader.Value);
            }}

            throw new JsonSerializationException($""Cannot deserialise {typeName} from {{reader.Value}}"");
        }}

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {{
            if (value is {typeName} id)
            {{
                writer.WriteValue(id.ToLong());
            }}
        }}
    }}
";
        }
    }
}
