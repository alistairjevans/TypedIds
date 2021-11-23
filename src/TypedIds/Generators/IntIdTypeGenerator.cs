using System.Text;
using TypedIds.Converters;

namespace TypedIds.Generators
{
    class IntIdTypeGenerator : IdTypeGenerator
    {
        public IntIdTypeGenerator()
            : base(new IntBsonSerializerGenerator(),
                   new ParsingTypeConverterGenerator(),
                   new IntNewtonsoftConverterGenerator(),
                   new IntSystemJsonConverterGenerator())
        {
        }

        protected override void FormatType(string typeName, TypeAttachmentMetadata metadata, StringBuilder builder)
        {        
            builder.Append($@"
    readonly partial struct {typeName} : IEquatable<{typeName}>
    {{
        private readonly int _backingId;

        /// <summary>
        /// Get the default zero state of {typeName}.
        /// </summary>
        public static {typeName} Zero {{ get; }} = new {typeName}(0);

        /// <summary>
        /// Create a new {typeName} from a 32-bit integer.
        /// </summary>
        public static {typeName} FromInt(int number) => new {typeName}(number);

        /// <summary>
        /// Try to parse a {typeName} from its string representation.
        /// </summary>
        public static bool TryParse(string text, out {typeName} id)
        {{
            if (int.TryParse(text, out var intId))
            {{
                id = new {typeName}(intId);
                return true;
            }}

            id = Zero;
            return false;
        }}

        private {typeName}(int number)
        {{
            _backingId = number;
        }}

        /// <summary>
        /// Get the int value backing this value.
        /// </summary>
        public int ToInt() => _backingId;

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
