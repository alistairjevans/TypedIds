using Microsoft.CodeAnalysis;
using System.Text;

namespace TypedIds.Generators
{
    internal abstract class IdTypeGenerator : BaseGeneratedTypeFormatter, ITypeGenerator
    {
        private readonly IConverterGenerator[] _converters;

        protected IdTypeGenerator(params IConverterGenerator[] converters)
        {
            _converters = converters;
        }

        public void CreateTypeExtension(GeneratorExecutionContext context, INamedTypeSymbol extendingTypeSymbol, GenerationOptions options)
        {
            var metadata = new TypeAttachmentMetadata();

            // We're going to need System regardless.
            metadata.AddNamespace("System");

            foreach (var converter in _converters)
            {
                if (converter.ShouldGenerate(context, extendingTypeSymbol, options))
                {
                    converter.AddSource(context, extendingTypeSymbol, metadata, options);
                }
            }

            var code = WrapSourceOutput(extendingTypeSymbol, FormatType(extendingTypeSymbol, metadata));

            context.AddSource(GetGeneratedFileName(extendingTypeSymbol, "Generated"), code);
        }

        private string FormatType(INamedTypeSymbol typeSymbol, TypeAttachmentMetadata metadata)
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

            FormatType(name, metadata, builder);

            return builder.ToString();
        }

        protected abstract void FormatType(string typeName, TypeAttachmentMetadata metadata, StringBuilder builder);
    }




}
