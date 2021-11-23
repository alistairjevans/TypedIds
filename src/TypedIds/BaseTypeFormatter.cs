using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Text;

#nullable enable

namespace TypedIds
{
    internal abstract class BaseGeneratedTypeFormatter
    {
        protected SourceText WrapWithNamespaceIfNeeded(INamedTypeSymbol extendingTypeSymbol, string source)
        {
            if (extendingTypeSymbol.ContainingNamespace is INamespaceSymbol nSpace && nSpace.Name != string.Empty)
            {
                var namespaceComponents = new Stack<string>();

                namespaceComponents.Push(nSpace.Name);

                while (nSpace.ContainingNamespace is object && nSpace.ContainingNamespace.Name != string.Empty)
                {
                    nSpace = nSpace.ContainingNamespace;

                    namespaceComponents.Push(nSpace.Name);
                }

                string fullname = string.Join(".", namespaceComponents);

                // In a namespace; wrap the formatted type as well.
                return SourceText.From($@"
namespace {fullname} {{
{source}
}}
                ", Encoding.UTF8);
            }

            return SourceText.From(source, Encoding.UTF8);
        }

        protected virtual string GetGeneratedFileName(INamedTypeSymbol symbol, string suffix)
        {
            if (symbol.ContainingNamespace is INamespaceSymbol nSpace && nSpace.Name != string.Empty)
            {
                return $"{nSpace.Name}.{symbol.Name}.{suffix}.cs";
            }

            return $"{symbol.Name}.{suffix}.cs";
        }
    }    
}
