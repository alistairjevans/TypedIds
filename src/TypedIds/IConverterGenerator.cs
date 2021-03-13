using Microsoft.CodeAnalysis;

namespace TypedIds
{
    public interface IConverterGenerator
    {
        bool ShouldGenerate(GeneratorExecutionContext context, INamedTypeSymbol generatingForType);

        void AddSource(GeneratorExecutionContext context, INamedTypeSymbol generatingForType, TypeAttachmentMetadata metadata);
    }
}
