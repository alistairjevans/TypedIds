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

        private static readonly IConverterGenerator[] _attributeBasedConverters = new IConverterGenerator[]
        {
            new GuidTypeConverterGenerator(),
            new GuidBsonSerializerGenerator(),
        };

        private const string AttributeSource = @"
using System;
namespace TypedIds
{
    [AttributeUsage(AttributeTargets.Struct)]
    internal sealed class TypedIdAttribute : Attribute
    {
    }
}
        ";

        public void Execute(GeneratorExecutionContext context)
        {
            var rx = (SyntaxReceiver)context.SyntaxContextReceiver!;

            // Go through each entry and add the appropriate types.
            
            foreach (var (owner, attribute) in rx.Types)
            {
                var semanticModel = context.Compilation.GetSemanticModel(owner.SyntaxTree);

                var structSymbol = semanticModel.GetDeclaredSymbol(owner);

                // First, validate.
                if (ValidateType(context, owner, structSymbol, attribute))
                {
                    var typeMetadata = new TypeAttachmentMetadata();

                    // We're going to need System at least.
                    typeMetadata.AddNamespace("System");

                    // Let our converts add things, and add additional metadata that goes into our generator.
                    foreach (var converter in _attributeBasedConverters)
                    {
                        if (converter.ShouldGenerate(context, structSymbol))
                        {
                            converter.AddSource(context, structSymbol, typeMetadata);
                        }
                    }

                    // Now actually generate the type.
                    var typeGenerator = new GuidTypeGenerator();

                    typeGenerator.CreateTypeExtension(context, structSymbol, typeMetadata);
                }
            }
        }

        private bool ValidateType(GeneratorExecutionContext context, StructDeclarationSyntax owner, INamedTypeSymbol symbol, AttributeSyntax attribute)
        {
            if (!owner.Modifiers.Any(SyntaxKind.PartialKeyword))
            {
                context.ReportDiagnostic(Diagnostic.Create(TypeIsNotPartial, owner.GetLocation(), symbol.Name));

                return false;
            }

            return true;
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForPostInitialization(pi => pi.AddSource("TypedId_Attrs_", AttributeSource));
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        private class SyntaxReceiver : ISyntaxContextReceiver
        {
            public List<(StructDeclarationSyntax Owner, AttributeSyntax Attribute)> Types { get; } = new();

            public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
            {
                var node = context.Node;

                if (context.Node is AttributeSyntax attrib &&
                    context.SemanticModel.GetTypeInfo(attrib).Type?.ToDisplayString() == "TypedIds.TypedIdAttribute")
                {
                    var owningStruct = attrib.FirstAncestorOrSelf<StructDeclarationSyntax>();

                    if (owningStruct is object)
                    {
                        var symbol = context.SemanticModel.GetSymbolInfo(owningStruct);

                        Types.Add((owningStruct, attrib));
                    }
                }
            }
        }
    }
}
