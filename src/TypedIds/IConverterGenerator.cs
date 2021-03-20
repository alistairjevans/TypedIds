using Microsoft.CodeAnalysis;

namespace TypedIds
{
    internal interface IConverterGenerator
    {
        bool ShouldGenerate(GeneratorExecutionContext context, INamedTypeSymbol generatingForType, GenerationOptions options);

        void AddSource(GeneratorExecutionContext context, INamedTypeSymbol generatingForType, TypeAttachmentMetadata metadata, GenerationOptions options);
    }
}
