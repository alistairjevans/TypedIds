using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace TypedIds.Converters
{
    internal class GuidBsonSerializerGenerator : BaseGeneratedTypeFormatter, IConverterGenerator
    {
        public bool ShouldGenerate(GeneratorExecutionContext context, INamedTypeSymbol generatingForType)
        {
            return context.Compilation.ReferencedAssemblyNames.Any(assemblyId => assemblyId.Name == "MongoDB.Bson");
        }

        public void AddSource(GeneratorExecutionContext context, INamedTypeSymbol generatingForType, TypeAttachmentMetadata metadata)
        {
            var code = WrapWithNamespaceIfNeeded(generatingForType, CreateSource(generatingForType));

            context.AddSource(GetGeneratedFileName(generatingForType, "BsonSerializer"), code);

            metadata.AddAttributeLiteral($"BsonSerializer(typeof({generatingForType.Name}GuidBsonSerialiser))");
            metadata.AddNamespace("MongoDB.Bson.Serialization.Attributes");
        }

        private string CreateSource(INamedTypeSymbol symbol)
        {
            var name = symbol.Name;

            return $@"
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Serializers;

    class {name}GuidBsonSerialiser : SerializerBase<{name}>
    {{
        private readonly GuidSerializer _guidSerialiser = new GuidSerializer(GuidRepresentation.Standard);

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, {name} value)
        {{
            _guidSerialiser.Serialize(context, args, value.ToGuid());
        }}

        public override {name} Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {{
            var guid = _guidSerialiser.Deserialize(context, args);

            return {name}.FromGuid(guid);
        }}
    }}
";
        }
    }
}
