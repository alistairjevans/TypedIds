using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Templates
{
    public class ReplaceMeGuidBsonSerialiser : SerializerBase<ReplaceMeGuidId>
    {
        private readonly GuidSerializer _guidSerialiser = new GuidSerializer(GuidRepresentation.Standard);

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ReplaceMeGuidId value)
        {
            _guidSerialiser.Serialize(context, args, value.ToGuid());
        }

        public override ReplaceMeGuidId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var guid = _guidSerialiser.Deserialize(context, args);

            return ReplaceMeGuidId.FromGuid(guid);
        }
    }
}
