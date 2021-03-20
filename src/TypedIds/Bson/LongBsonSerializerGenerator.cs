using System;

namespace TypedIds.Converters
{

    internal class LongBsonSerializerGenerator : BaseBsonSerializerGenerator, IConverterGenerator
    {
        protected override string CreateSource(string typeName)
        {
            return $@"
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Serializers;

    class {typeName}BsonSerialiser : SerializerBase<{typeName}>
    {{
        private readonly Int64Serializer _intSerializer = new Int64Serializer();

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, {typeName} value)
        {{
            _intSerializer.Serialize(context, args, value.ToLong());
        }}

        public override {typeName} Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {{
            var number = _intSerializer.Deserialize(context, args);

            return {typeName}.FromLong(number);
        }}
    }}
";
        }
    }
}
