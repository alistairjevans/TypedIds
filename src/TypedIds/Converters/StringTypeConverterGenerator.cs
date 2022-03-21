using Microsoft.CodeAnalysis;

namespace TypedIds.Converters
{
    internal class StringTypeConverterGenerator : BaseGeneratedTypeFormatter, IConverterGenerator
    {
        public bool ShouldGenerate(GeneratorExecutionContext context, INamedTypeSymbol generatingForType, GenerationOptions options)
        {
            // Can always generate a TypeConverter for strings.
            return true;
        }

        public void AddSource(GeneratorExecutionContext context, INamedTypeSymbol generatingForType, TypeAttachmentMetadata metadata, GenerationOptions options)
        {
            var code = WrapSourceOutput(generatingForType, CreateSource(generatingForType));

            context.AddSource(GetGeneratedFileName(generatingForType, "TypeConverter"), code);

            metadata.AddAttributeLiteral($"TypeConverter(typeof({generatingForType.Name}.{generatingForType.Name}TypeConverter))");
            metadata.AddNamespace("System.ComponentModel");
        }

        private string CreateSource(INamedTypeSymbol symbol)
        {
            var name = symbol.Name;

            return $@"
    using System;
    using System.ComponentModel;
    using System.Globalization;

    partial struct {name} 
    {{
        public class {name}TypeConverter: TypeConverter
        {{
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {{
                return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
            }}

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {{
                if (value is string text)
                {{
                    return {name}.FromString(text);
                }}

                return base.ConvertFrom(context, culture, value);
            }}
        }}
    }}
";
        }
    }
}
