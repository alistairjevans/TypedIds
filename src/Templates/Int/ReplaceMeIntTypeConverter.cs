using System;
using System.ComponentModel;
using System.Globalization;

namespace Templates
{
    class ReplaceMeIntTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string text && ReplaceMeIntId.TryParse(text, out var id))
            {
                return id;
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
