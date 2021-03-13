using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel;

namespace TypedIds.Unit
{
    [BsonSerializer(typeof(MongoDBGuidSerialiser))]
    [TypeConverter(typeof(MyIdTypeConverter))]
    public readonly partial struct MyId : IEquatable<MyId>
    {
        private readonly Guid _backingId;

        public static MyId Empty { get; } = new MyId(Guid.Empty);

        public static MyId FromGuid(Guid guid) => new MyId(guid);

        public static MyId New() => new MyId(Guid.NewGuid());

        public static bool TryParse(string text, out MyId id)
        {
            if (Guid.TryParseExact(text, "N", out var guidId))
            {
                id = new MyId(guidId);

                return true;
            }

            id = Empty;
            return false;
        }

        private MyId(Guid guid)
        {
            _backingId = guid;
        }

        public Guid ToGuid() => _backingId;

        public override int GetHashCode() => _backingId.GetHashCode();

        public override string ToString() => _backingId.ToString("N");

        public override bool Equals(object obj)
        {
            if (obj is MyId otherId)
            {
                return Equals(otherId);
            }

            return false;
        }

        public bool Equals(MyId other) => _backingId.Equals(other._backingId);

        public static bool operator ==(MyId left, MyId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MyId left, MyId right)
        {
            return !(left == right);
        }
    }
}
