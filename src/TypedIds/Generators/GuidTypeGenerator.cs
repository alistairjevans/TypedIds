using Microsoft.CodeAnalysis;
using System.Text;

namespace TypedIds.Generators
{
    class GuidTypeGenerator : BaseGeneratedTypeFormatter, ITypeGenerator
    {
        public void CreateTypeExtension(GeneratorExecutionContext context, INamedTypeSymbol extendingTypeSymbol, TypeAttachmentMetadata metadata)
        {
            var code = WrapWithNamespaceIfNeeded(extendingTypeSymbol, FormatType(extendingTypeSymbol, metadata));

            context.AddSource(GetGeneratedFileName(extendingTypeSymbol, "Generated"), code);
        }

        protected string FormatType(INamedTypeSymbol typeSymbol, TypeAttachmentMetadata metadata)
        {
            var builder = new StringBuilder();

            var name = typeSymbol.Name;

            foreach (var usingStatement in metadata.AdditionalNamespaces)
            {
                builder.Append($@"
    using {usingStatement};");
            }

            foreach (var attr in metadata.AttributeLiterals)
            {
                builder.Append($@"
    [{attr}]");
            }

            builder.Append($@"
    readonly partial struct {name} : IEquatable<{name}>
    {{
        private readonly Guid _backingId;

        /// <summary>
        /// Get the default empty state of {name}.
        /// </summary>
        public static {name} Empty {{ get; }} = new {name}(Guid.Empty);

        /// <summary>
        /// Creates a new {name} from a standard Guid.
        /// </summary>
        public static {name} FromGuid(Guid guid) => new {name}(guid);

        /// <summary>
        /// Create a new ID with a random unique value.
        /// </summary>
        public static {name} New() => new {name}(Guid.NewGuid());

        /// <summary>
        /// Attempt to parse a {name} from a string.
        /// </summary>
        public static bool TryParse(string text, out {name} id)
        {{
            if (Guid.TryParseExact(text, ""N"", out var guidId))
            {{
                id = new {name}(guidId);

                return true;
            }}

            id = Empty;
            return false;
        }}

        private {name}(Guid guid)
        {{
            _backingId = guid;
        }}

        /// <summary>
        /// Get the underlying guid from this {name}.
        /// </summary>
        public Guid ToGuid() => _backingId;

        public override int GetHashCode() => _backingId.GetHashCode();

        public override string ToString() => _backingId.ToString(""N"");

        public override bool Equals(object obj)
        {{
            if (obj is {name} otherId)
            {{
                return Equals(otherId);
            }}

            return false;
        }}

        public bool Equals({name} other) => _backingId.Equals(other._backingId);
    }}
");

            return builder.ToString();
        }
    }




}
