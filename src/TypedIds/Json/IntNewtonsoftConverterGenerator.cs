using System;

namespace TypedIds.Converters
{
    internal class IntNewtonsoftConverterGenerator : BaseNewtonsoftConverterGenerator, IConverterGenerator
    {
        protected override string CreateSource(string typeName)
        {
            return $@"
    using System;
    using Newtonsoft.Json;

    class {typeName}NsJsonConverter : JsonConverter
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
                return {typeName}.FromInt((int)reader.Value);
            }}

            if (reader.ValueType == typeof(long))
            {{
                return {typeName}.FromInt((int)(long)reader.Value);
            }}

            throw new JsonSerializationException($""Cannot deserialise {typeName} from {{reader.Value}}"");
        }}

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {{
            if (value is {typeName} id)
            {{
                writer.WriteValue(id.ToInt());
            }}
        }}
    }}
";
        }
    }
}
