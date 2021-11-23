namespace Templates.Int
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    class ReplaceMeIntIdJsonConverter : JsonConverter<ReplaceMeIntId>
    {
        public override ReplaceMeIntId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return ReplaceMeIntId.FromInt(reader.GetInt32());
        }

        public override void Write(Utf8JsonWriter writer, ReplaceMeIntId value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.ToInt());
        }
    }
}
