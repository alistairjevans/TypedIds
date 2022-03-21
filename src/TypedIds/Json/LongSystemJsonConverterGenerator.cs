using System;

namespace TypedIds.Converters
{
    internal class LongSystemJsonConverterGenerator : BaseSystemJsonConverterGenerator, IConverterGenerator
    {
        protected override string CreateSource(string typeName)
        {
            return $@"
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    partial struct {typeName} 
    {{
        public class {typeName}SystemJsonConverter : JsonConverter<{typeName}>
        {{
            public override {typeName} Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {{
                return {typeName}.FromLong(reader.GetInt64());
            }}

            public override void Write(Utf8JsonWriter writer, {typeName} value, JsonSerializerOptions options)
            {{
                writer.WriteNumberValue(value.ToLong());
            }}
        }}
    }}
";
        }
    }
}
