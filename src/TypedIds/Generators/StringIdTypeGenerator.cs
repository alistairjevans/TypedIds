using System.Text;
using TypedIds.Converters;

namespace TypedIds.Generators
{
    internal class StringIdTypeGenerator : IdTypeGenerator
    {
        public StringIdTypeGenerator()
            : base(new StringBsonSerializerGenerator(),
                   new StringTypeConverterGenerator(),
                   new StringSystemJsonConverterGenerator())
        {
        }

        protected override void FormatType(string typeName, TypeAttachmentMetadata metadata, StringBuilder builder)
        {        
            builder.Append($@"
    readonly partial struct {typeName} : IEquatable<{typeName}>
    {{
        private readonly string _backingId;

        /// <summary>
        /// Defines an empty value for <see cref=""{typeName}"" />.
        /// </summary>
        public static {typeName} Empty {{ get; }} = new {typeName}(string.Empty);

        /// <summary>
        /// Creates a <see cref=""{typeName}"" /> from a string.
        /// </summary>
        public static {typeName} FromString(string content) => new {typeName}(content);

        private {typeName}(string content)
        {{
            _backingId = content;
        }}

        /// <inheritdoc />
        public override int GetHashCode() => _backingId.GetHashCode();

        /// <inheritdoc />
        public override string ToString() => _backingId;

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
        public bool Equals({typeName} other) => _backingId.Equals(other._backingId, StringComparison.Ordinal);

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
