using System;

namespace TypedIds.Converters
{
    internal class GuidSystemJsonConverterGenerator : BaseSystemJsonConverterGenerator, IConverterGenerator
    {
        protected override string CreateSource(string typeName)
        {
            return $@"
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    class {typeName}SystemJsonConverter : JsonConverter<{typeName}>
    {{
        public override {typeName} Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {{
            var guid = reader.GetString();

            if ({typeName}.TryParse(guid, out var result))
            {{
                return result;
            }}

            throw new JsonException(""Cannot parse {typeName}"");
        }}

        public override void Write(Utf8JsonWriter writer, {typeName} value, JsonSerializerOptions options)
        {{
            writer.WriteStringValue(value.ToString());
        }}
    }}
";
        }
    }
}
