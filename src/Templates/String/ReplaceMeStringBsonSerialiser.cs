using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Templates
{
    public class ReplaceMeStringBsonSerialiser : SerializerBase<ReplaceMeStringId>
    {
        private readonly StringSerializer _strSerializer = new StringSerializer();

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ReplaceMeStringId value)
        {
           _strSerializer.Serialize(context, args, value.ToString());
        }

        public override ReplaceMeStringId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var text = _strSerializer.Deserialize(context, args);

            return ReplaceMeStringId.FromString(text ?? string.Empty);
        }
    }
}
