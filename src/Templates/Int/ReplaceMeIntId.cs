using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using Templates.Int;

namespace Templates
{
    [JsonConverter(typeof(ReplaceMeIntIdNewtonSoftConverter))]
    [BsonSerializer(typeof(ReplaceMeStringBsonSerialiser))]
    [TypeConverter(typeof(ReplaceMeIntTypeConverter))]
    public readonly partial struct ReplaceMeIntId : IEquatable<ReplaceMeIntId>
    {
        private readonly int _backingId;

        public static ReplaceMeIntId Zero { get; } = new ReplaceMeIntId(0);

        public static ReplaceMeIntId FromInt(int number) => new ReplaceMeIntId(number);

        public static bool TryParse(string text, out ReplaceMeIntId id)
        {
            if (int.TryParse(text, out var intId))
            {
                id = new ReplaceMeIntId(intId);
                return true;
            }

            id = Zero;
            return false;
        }

        private ReplaceMeIntId(int number)
        {
            _backingId = number;
        }

        public int ToInt() => _backingId;

        public override int GetHashCode() => _backingId.GetHashCode();

        public override string ToString() => _backingId.ToString();

        public override bool Equals(object obj)
        {
            if (obj is ReplaceMeIntId otherId)
            {
                return Equals(otherId);
            }

            return false;
        }

        public bool Equals(ReplaceMeIntId other) => _backingId.Equals(other._backingId);

        public static bool operator ==(ReplaceMeIntId left, ReplaceMeIntId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ReplaceMeIntId left, ReplaceMeIntId right)
        {
            return !(left == right);
        }
    }
}
