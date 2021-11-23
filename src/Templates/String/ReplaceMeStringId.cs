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

        /// <inheritdoc />
        public override int GetHashCode() => _backingId.GetHashCode();

        /// <inheritdoc />
        public override string ToString() => _backingId;

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is ReplaceMeStringId otherId)
            {
                return Equals(otherId);
            }

            return false;
        }

        /// <inheritdoc />
        public bool Equals(ReplaceMeStringId other) => _backingId.Equals(other._backingId, StringComparison.Ordinal);

        /// <inheritdoc />
        public static bool operator ==(ReplaceMeStringId left, ReplaceMeStringId right)
        {
            return left.Equals(right);
        }

        /// <inheritdoc />
        public static bool operator !=(ReplaceMeStringId left, ReplaceMeStringId right)
        {
            return !(left == right);
        }
    }
}
