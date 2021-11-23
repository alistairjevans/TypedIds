﻿using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Templates
{
    class ReplaceMeGuidSystemJsonConverter : JsonConverter<ReplaceMeGuidId>
    {
        public override ReplaceMeGuidId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var guid = reader.GetString();

            if (ReplaceMeGuidId.TryParse(guid, out var result))
            {
                return result;
            }

            throw new JsonException("Cannot parse ReplaceMeGuidId");
        }

        public override void Write(Utf8JsonWriter writer, ReplaceMeGuidId value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
