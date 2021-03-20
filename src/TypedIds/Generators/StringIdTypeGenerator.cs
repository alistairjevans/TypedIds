using System.Text;
using TypedIds.Converters;

namespace TypedIds.Generators
{
    internal class StringIdTypeGenerator : IdTypeGenerator
    {
        public StringIdTypeGenerator()
            : base(new StringBsonSerializerGenerator(),
                   new StringTypeConverterGenerator())
        {
        }

        protected override void FormatType(string typeName, TypeAttachmentMetadata metadata, StringBuilder builder)
        {        
            builder.Append($@"
    readonly partial struct {typeName} : IEquatable<{typeName}>
    {{
        private readonly string _backingId;

        public static {typeName} Empty {{ get; }} = new {typeName}(string.Empty);

        public static {typeName} FromString(string content) => new {typeName}(content);

        private {typeName}(string content)
        {{
            _backingId = content;
        }}

        public override int GetHashCode() => _backingId.GetHashCode();

        public override string ToString() => _backingId;

        public override bool Equals(object obj)
        {{
            if (obj is {typeName} otherId)
            {{
                return Equals(otherId);
            }}

            return false;
        }}

        public bool Equals({typeName} other) => _backingId.Equals(other._backingId, StringComparison.Ordinal);

        public static bool operator ==({typeName} left, {typeName} right)
        {{
            return left.Equals(right);
        }}

        public static bool operator !=({typeName} left, {typeName} right)
        {{
            return !(left == right);
        }}
    }}
");
        }
    }




}
