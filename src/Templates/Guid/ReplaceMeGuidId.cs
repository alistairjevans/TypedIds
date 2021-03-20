using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel;

namespace Templates
{
    [BsonSerializer(typeof(ReplaceMeGuidBsonSerialiser))]
    [TypeConverter(typeof(ReplaceMeGuidTypeConverter))]
    public readonly partial struct ReplaceMeGuidId : IEquatable<ReplaceMeGuidId>
    {
        private readonly Guid _backingId;

        public static ReplaceMeGuidId Empty { get; } = new ReplaceMeGuidId(Guid.Empty);

        public static ReplaceMeGuidId FromGuid(Guid guid) => new ReplaceMeGuidId(guid);

        public static ReplaceMeGuidId New() => new ReplaceMeGuidId(Guid.NewGuid());

        public static bool TryParse(string text, out ReplaceMeGuidId id)
        {
            if (Guid.TryParseExact(text, "N", out var guidId))
            {
                id = new ReplaceMeGuidId(guidId);
                return true;
            }

            id = Empty;
            return false;
        }

        private ReplaceMeGuidId(Guid guid)
        {
            _backingId = guid;
        }

        public Guid ToGuid() => _backingId;

        public override int GetHashCode() => _backingId.GetHashCode();

        public override string ToString() => _backingId.ToString("N");

        public override bool Equals(object obj)
        {
            if (obj is ReplaceMeGuidId otherId)
            {
                return Equals(otherId);
            }

            return false;
        }

        public bool Equals(ReplaceMeGuidId other) => _backingId.Equals(other._backingId);

        public static bool operator ==(ReplaceMeGuidId left, ReplaceMeGuidId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ReplaceMeGuidId left, ReplaceMeGuidId right)
        {
            return !(left == right);
        }
    }
}
