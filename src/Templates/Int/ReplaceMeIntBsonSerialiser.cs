using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Templates
{
    public class ReplaceMeIntBsonSerialiser : SerializerBase<ReplaceMeIntId>
    {
        private readonly Int32Serializer _intSerializer = new Int32Serializer();

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ReplaceMeIntId value)
        {
            _intSerializer.Serialize(context, args, value.ToInt());
        }

        public override ReplaceMeIntId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var number = _intSerializer.Deserialize(context, args);

            return ReplaceMeIntId.FromInt(number);
        }
    }
}
