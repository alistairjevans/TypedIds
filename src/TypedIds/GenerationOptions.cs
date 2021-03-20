using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace TypedIds
{
    internal readonly struct GenerationOptions
    {
        public static GenerationOptions FromAttribute(AttributeSyntax attrib, SemanticModel semanticModel)
        {
            var backingType = IdBackingType.Guid;

            if (attrib.ArgumentList is object && attrib.ArgumentList.Arguments.Any())
            {
                // Only argument right now is the backing type.
                var argValue = semanticModel.GetConstantValue(attrib.ArgumentList.Arguments.First().Expression);

                if (argValue.HasValue)
                {
                    backingType = (IdBackingType)argValue.Value;
                }
            }

            return new GenerationOptions(backingType);
        }

        private GenerationOptions(IdBackingType backingType)
        {
            BackingType = backingType;
        }

        public IdBackingType BackingType { get; }
    }
}
