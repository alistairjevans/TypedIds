using Microsoft.CodeAnalysis;

namespace TypedIds.Converters
{
    internal class TypeConverterGenerator : BaseGeneratedTypeFormatter, IConverterGenerator
    {
        public bool ShouldGenerate(GeneratorExecutionContext context, INamedTypeSymbol generatingForType, GenerationOptions options)
        {
            // Can always generate a TypeConverter for Guids.
            return true;
        }

        public void AddSource(GeneratorExecutionContext context, INamedTypeSymbol generatingForType, TypeAttachmentMetadata metadata, GenerationOptions options)
        {
            var code = WrapWithNamespaceIfNeeded(generatingForType, CreateSource(generatingForType));

            context.AddSource(GetGeneratedFileName(generatingForType, "TypeConverter"), code);

            metadata.AddAttributeLiteral($"TypeConverter(typeof({generatingForType.Name}TypeConverter))");
            metadata.AddNamespace("System.ComponentModel");
        }

        private string CreateSource(INamedTypeSymbol symbol)
        {
            var name = symbol.Name;

            return $@"
    using System;
    using System.ComponentModel;
    using System.Globalization;

    class {name}TypeConverter: TypeConverter
    {{
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {{
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }}

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {{
            if (value is string text && {name}.TryParse(text, out var id))
            {{
                return id;
            }}

            return base.ConvertFrom(context, culture, value);
        }}
    }}
";
        }
    }
}
