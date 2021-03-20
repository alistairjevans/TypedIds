using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using TypedIds.Converters;
using TypedIds.Generators;

namespace TypedIds
{
    [Generator]
    public class Generator : ISourceGenerator
    {
        private static readonly DiagnosticDescriptor TypeIsNotPartial = new DiagnosticDescriptor(id: "TYPEDID001",
                                                                                                 title: "Decorated type must be partial",
                                                                                                 messageFormat: "Decorated type {0} must be partial",
                                                                                                 category: "TypedIds",
                                                                                                 DiagnosticSeverity.Error,
                                                                                                 isEnabledByDefault: true);

        private static readonly ITypeGenerator GuidGenerator = new GuidIdTypeGenerator();
        private static readonly ITypeGenerator IntGenerator = new IntIdTypeGenerator();
        private static readonly ITypeGenerator LongGenerator = new LongIdTypeGenerator();

        private const string AttributeSource = @"
using System;
namespace TypedIds
{
    [AttributeUsage(AttributeTargets.Struct)]
    internal sealed class TypedIdAttribute : Attribute
    {
        public TypedIdAttribute(IdBackingType backingType = IdBackingType.Guid)
        {
            BackingType = backingType;
        }

        public IdBackingType BackingType { get; }
    }
}
        ";

        private static readonly string IdBackingTypeSource = EnumRenderer.RenderEnumToSource<IdBackingType>();

        public void Execute(GeneratorExecutionContext context)
        {
            var rx = (SyntaxReceiver)context.SyntaxContextReceiver!;

#pragma warning disable RS1024 // Compare symbols correctly (https://github.com/dotnet/roslyn-analyzers/issues/4469)
            var trackSeenTypes = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
#pragma warning restore RS1024 // Compare symbols correctly

            // Go through each entry and add the appropriate types.            
            foreach (var (node, options) in rx.Types)
            {
                var semanticModel = context.Compilation.GetSemanticModel(node.SyntaxTree);

                var structSymbol = semanticModel.GetDeclaredSymbol(node);

                if (!trackSeenTypes.Add(structSymbol))
                {
                    // Seen already - duplicate attribute usage warning is going to show up.
                    continue;
                }

                // First, validate.
                if (ValidateType(context, node, structSymbol, options))
                {
                    // Use the backing type to pick the generator.
                    ITypeGenerator generator = options.BackingType switch
                    {
                        IdBackingType.Guid => GuidGenerator,
                        IdBackingType.Int => IntGenerator,
                        IdBackingType.Long => LongGenerator,
                        _ => null,
                    };

                    if (generator is ITypeGenerator)
                    {
                        generator.CreateTypeExtension(context, structSymbol, options);
                    }
                }
            }
        }

        private bool ValidateType(GeneratorExecutionContext context, StructDeclarationSyntax owner, INamedTypeSymbol symbol, GenerationOptions options)
        {
            if (!owner.Modifiers.Any(SyntaxKind.PartialKeyword))
            {
                context.ReportDiagnostic(Diagnostic.Create(TypeIsNotPartial, owner.Identifier.GetLocation(), symbol.Name));

                return false;
            }

            return true;
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForPostInitialization(pi => {
                pi.AddSource("TypedId_Attrs_", AttributeSource);
                pi.AddSource("TypedId_Enums_IdBackingType", IdBackingTypeSource);
            });
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        private class SyntaxReceiver : ISyntaxContextReceiver
        {
            public List<(StructDeclarationSyntax Node, GenerationOptions Options)> Types { get; } = new();

            public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
            {
                var node = context.Node;

                if (context.Node is AttributeSyntax attrib &&
                    context.SemanticModel.GetTypeInfo(attrib).Type?.ToDisplayString() == "TypedIds.TypedIdAttribute")
                {
                    var owningStruct = attrib.FirstAncestorOrSelf<StructDeclarationSyntax>();

                    if (owningStruct is object)
                    {
                        Types.Add((owningStruct, GenerationOptions.FromAttribute(attrib, context.SemanticModel)));
                    }
                }
            }
        }
    }
}
