using Microsoft.CodeAnalysis;

#nullable enable

namespace TypedIds
{
    internal interface ITypeGenerator
    {
        void CreateTypeExtension(GeneratorExecutionContext context, INamedTypeSymbol extendingTypeSymbol, GenerationOptions options);
    }
}
