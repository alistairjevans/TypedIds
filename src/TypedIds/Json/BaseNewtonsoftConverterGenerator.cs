using Microsoft.CodeAnalysis;
using System.Linq;

namespace TypedIds.Converters
{
    internal abstract class BaseNewtonsoftConverterGenerator : BaseGeneratedTypeFormatter, IConverterGenerator
    {
        public bool ShouldGenerate(GeneratorExecutionContext context, INamedTypeSymbol generatingForType, GenerationOptions options)
        {
            return context.Compilation.ReferencedAssemblyNames.Any(assemblyId => assemblyId.Name == "Newtonsoft.Json");
        }

        public void AddSource(GeneratorExecutionContext context, INamedTypeSymbol generatingForType, TypeAttachmentMetadata metadata, GenerationOptions options)
        {
            var code = WrapWithNamespaceIfNeeded(generatingForType, CreateSource(generatingForType.Name));

            context.AddSource(GetGeneratedFileName(generatingForType, "NsJsonConverter"), code);

            metadata.AddAttributeLiteral($"Newtonsoft.Json.JsonConverter(typeof({generatingForType.Name}NsJsonConverter))");
        }

        protected abstract string CreateSource(string typeName);
    }
}
