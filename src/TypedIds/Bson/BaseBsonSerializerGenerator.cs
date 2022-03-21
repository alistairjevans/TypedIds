using Microsoft.CodeAnalysis;
using System.Linq;

namespace TypedIds.Converters
{
    internal abstract class BaseBsonSerializerGenerator : BaseGeneratedTypeFormatter, IConverterGenerator
    {
        public bool ShouldGenerate(GeneratorExecutionContext context, INamedTypeSymbol generatingForType, GenerationOptions options)
        {
            return context.Compilation.ReferencedAssemblyNames.Any(assemblyId => assemblyId.Name == "MongoDB.Bson");
        }

        public void AddSource(GeneratorExecutionContext context, INamedTypeSymbol generatingForType, TypeAttachmentMetadata metadata, GenerationOptions options)
        {
            var code = WrapSourceOutput(generatingForType, CreateSource(generatingForType.Name));

            context.AddSource(GetGeneratedFileName(generatingForType, "BsonSerializer"), code);

            metadata.AddAttributeLiteral($"BsonSerializer(typeof({generatingForType.Name}.{generatingForType.Name}BsonSerialiser))");
            metadata.AddNamespace("MongoDB.Bson.Serialization.Attributes");
        }

        protected abstract string CreateSource(string typeName);
    }
}
