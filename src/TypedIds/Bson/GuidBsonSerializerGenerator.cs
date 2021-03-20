namespace TypedIds.Converters
{
    internal class GuidBsonSerializerGenerator : BaseBsonSerializerGenerator, IConverterGenerator
    {
        protected override string CreateSource(string typeName)
        {
            return $@"
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Serializers;

    class {typeName}BsonSerialiser : SerializerBase<{typeName}>
    {{
        private readonly GuidSerializer _guidSerialiser = new GuidSerializer(GuidRepresentation.Standard);

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, {typeName} value)
        {{
            _guidSerialiser.Serialize(context, args, value.ToGuid());
        }}

        public override {typeName} Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {{
            var guid = _guidSerialiser.Deserialize(context, args);

            return {typeName}.FromGuid(guid);
        }}
    }}
";
        }
    }
}
