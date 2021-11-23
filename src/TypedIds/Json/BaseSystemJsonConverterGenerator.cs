using Microsoft.CodeAnalysis;
using System.Linq;

namespace TypedIds.Converters
{
    internal abstract class BaseSystemJsonConverterGenerator : BaseGeneratedTypeFormatter, IConverterGenerator
    {
        private bool? _cachedShouldGenerate;

        public bool ShouldGenerate(GeneratorExecutionContext context, INamedTypeSymbol generatingForType, GenerationOptions options)
        {
            if (!_cachedShouldGenerate.HasValue)
            {
                var allInstances = context.Compilation.References
                    .Select(context.Compilation.GetAssemblyOrModuleSymbol)
                    .OfType<IAssemblySymbol>()
                    .Select(assemblySymbol => assemblySymbol.GetTypeByMetadataName("System.Text.Json.Serialization.JsonConverter"))
                    .Where(t => t != null);

                _cachedShouldGenerate = allInstances.Any();
            }

            return _cachedShouldGenerate.Value;
        }

        public void AddSource(GeneratorExecutionContext context, INamedTypeSymbol generatingForType, TypeAttachmentMetadata metadata, GenerationOptions options)
        {
            var code = WrapSourceOutput(generatingForType, CreateSource(generatingForType.Name));

            context.AddSource(GetGeneratedFileName(generatingForType, "SystemJsonConverter"), code);

            metadata.AddAttributeLiteral($"System.Text.Json.Serialization.JsonConverter(typeof({generatingForType.Name}SystemJsonConverter))");
        }

        protected abstract string CreateSource(string typeName);
    }
}
