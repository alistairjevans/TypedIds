using System.Text;
using TypedIds.Converters;

namespace TypedIds.Generators
{
    internal class GuidIdTypeGenerator : IdTypeGenerator
    {
        public GuidIdTypeGenerator()
            : base(new GuidBsonSerializerGenerator(),
                   new ParsingTypeConverterGenerator())
        {
        }

        protected override void FormatType(string typeName, TypeAttachmentMetadata metadata, StringBuilder builder)
        {        
            builder.Append($@"
    readonly partial struct {typeName} : IEquatable<{typeName}>
    {{
        private readonly Guid _backingId;

        /// <summary>
        /// Get the default empty state of {typeName}.
        /// </summary>
        public static {typeName} Empty {{ get; }} = new {typeName}(Guid.Empty);

        /// <summary>
        /// Creates a new {typeName} from a standard Guid.
        /// </summary>
        public static {typeName} FromGuid(Guid guid) => new {typeName}(guid);

        /// <summary>
        /// Create a new ID with a random unique value.
        /// </summary>
        public static {typeName} New() => new {typeName}(Guid.NewGuid());

        /// <summary>
        /// Attempt to parse a {typeName} from a string.
        /// </summary>
        public static bool TryParse(string text, out {typeName} id)
        {{
            if (Guid.TryParseExact(text, ""N"", out var guidId))
            {{
                id = new {typeName}(guidId);

                return true;
            }}

            id = Empty;
            return false;
        }}

        private {typeName}(Guid guid)
        {{
            _backingId = guid;
        }}

        /// <summary>
        /// Get the underlying guid from this {typeName}.
        /// </summary>
        public Guid ToGuid() => _backingId;

        public override int GetHashCode() => _backingId.GetHashCode();

        public override string ToString() => _backingId.ToString(""N"");

        public override bool Equals(object obj)
        {{
            if (obj is {typeName} otherId)
            {{
                return Equals(otherId);
            }}

            return false;
        }}

        public bool Equals({typeName} other) => _backingId.Equals(other._backingId);

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
