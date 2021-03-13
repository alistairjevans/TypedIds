using Microsoft.CodeAnalysis;

#nullable enable

namespace TypedIds
{
    public interface ITypeGenerator
    {
        void CreateTypeExtension(GeneratorExecutionContext context, INamedTypeSymbol extendingTypeSymbol, TypeAttachmentMetadata metadata);
    }
}
