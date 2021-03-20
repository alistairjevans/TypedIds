using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel;

namespace Templates
{
    [BsonSerializer(typeof(ReplaceMeStringBsonSerialiser))]
    [TypeConverter(typeof(ReplaceMeIntTypeConverter))]
    public readonly partial struct ReplaceMeStringId : IEquatable<ReplaceMeStringId>
    {
        private readonly string _backingId;

        public static ReplaceMeStringId Empty { get; } = new ReplaceMeStringId(string.Empty);

        public static ReplaceMeStringId FromString(string content) => new ReplaceMeStringId(content);

        private ReplaceMeStringId(string content)
        {
            _backingId = content;
        }

        public override int GetHashCode() => _backingId.GetHashCode();

        public override string ToString() => _backingId;

        public override bool Equals(object obj)
        {
            if (obj is ReplaceMeStringId otherId)
            {
                return Equals(otherId);
            }

            return false;
        }

        public bool Equals(ReplaceMeStringId other) => _backingId.Equals(other._backingId, StringComparison.Ordinal);

        public static bool operator ==(ReplaceMeStringId left, ReplaceMeStringId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ReplaceMeStringId left, ReplaceMeStringId right)
        {
            return !(left == right);
        }
    }
}
