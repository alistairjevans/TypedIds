namespace TypedIds.Converters
{
    internal class StringBsonSerializerGenerator : BaseBsonSerializerGenerator, IConverterGenerator
    {
        protected override string CreateSource(string typeName)
        {
            return $@"
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Serializers;

    class {typeName}BsonSerialiser : SerializerBase<{typeName}>
    {{
        private readonly StringSerializer _strSerializer = new StringSerializer();

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, {typeName} value)
        {{
           _strSerializer.Serialize(context, args, value.ToString());
        }}

        public override {typeName} Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {{
            var text = _strSerializer.Deserialize(context, args);

            return {typeName}.FromString(text ?? string.Empty);
        }}
    }}
";
        }
    }
}
