using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace TypedIds.Unit
{
    public class MongoDBGuidSerialiser : SerializerBase<MyId>
    {
        private readonly GuidSerializer _guidSerialiser = new GuidSerializer(GuidRepresentation.Standard);

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, MyId value)
        {
            _guidSerialiser.Serialize(context, args, value.ToGuid());
        }

        public override MyId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var guid = _guidSerialiser.Deserialize(context, args);

            return MyId.FromGuid(guid);
        }
    }
}
