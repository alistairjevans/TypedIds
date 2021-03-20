using System;
using System.ComponentModel;
using System.Globalization;

namespace Templates
{
    class ReplaceMeStringTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string text)
            {
                return ReplaceMeStringId.FromString(text);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
