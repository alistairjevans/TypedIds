using System.Text;
using TypedIds.Converters;

namespace TypedIds.Generators
{
    class LongIdTypeGenerator : IdTypeGenerator
    {
        public LongIdTypeGenerator()
            : base(new LongBsonSerializerGenerator(),
                   new ParsingTypeConverterGenerator(),
                   new LongNewtonsoftConverterGenerator(),
                   new LongSystemJsonConverterGenerator())
        {
        }

        protected override void FormatType(string typeName, TypeAttachmentMetadata metadata, StringBuilder builder)
        {        
            builder.Append($@"
    readonly partial struct {typeName} : IEquatable<{typeName}>
    {{
        private readonly long _backingId;

        /// <summary>
        /// Get the default zero state of {typeName}.
        /// </summary>
        public static {typeName} Zero {{ get; }} = new {typeName}(0);

        /// <summary>
        /// Create a new {typeName} from a 64-bit integer.
        /// </summary>
        public static {typeName} FromLong(long number) => new {typeName}(number);

        /// <summary>
        /// Try to parse a {typeName} from its string representation.
        /// </summary>
        public static bool TryParse(string text, out {typeName} id)
        {{
            if (long.TryParse(text, out var longId))
            {{
                id = new {typeName}(longId);
                return true;
            }}

            id = Zero;
            return false;
        }}

        private {typeName}(long number)
        {{
            _backingId = number;
        }}

        /// <summary>
        /// Get the long value backing this value.
        /// </summary>
        public long ToLong() => _backingId;

        /// <inheritdoc />
        public override int GetHashCode() => _backingId.GetHashCode();

        /// <inheritdoc />
        public override string ToString() => _backingId.ToString();

        /// <inheritdoc />
        public override bool Equals(object obj)
        {{
            if (obj is {typeName} otherId)
            {{
                return Equals(otherId);
            }}

            return false;
        }}

        /// <inheritdoc />
        public bool Equals({typeName} other) => _backingId.Equals(other._backingId);

        /// <inheritdoc />
        public static bool operator ==({typeName} left, {typeName} right)
        {{
            return left.Equals(right);
        }}

        /// <inheritdoc />
        public static bool operator !=({typeName} left, {typeName} right)
        {{
            return !(left == right);
        }}
    }}
");
        }
    }




}
