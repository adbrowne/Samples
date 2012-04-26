namespace WidgetCreatorMvc3.Service.Core
{
    using System;
    using System.ComponentModel;

    [TypeConverter(typeof(WidgetIdTypeConverter))]
    public class WidgetId
    {
        public Guid Id { get; private set; }

        public WidgetId(Guid id)
        {
            this.Id = id;
        }

        public override string ToString()
        {
            return Id.ToString();
        }

        public bool Equals(WidgetId other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return other.Id.Equals(this.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof(WidgetId))
            {
                return false;
            }
            return Equals((WidgetId)obj);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }

    public class WidgetIdTypeConverter : TypeConverter
    {
        public WidgetIdTypeConverter()
        {
            
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return base.ConvertFrom(context, culture, value);
        }

        public override object CreateInstance(ITypeDescriptorContext context, System.Collections.IDictionary propertyValues)
        {
            return base.CreateInstance(context, propertyValues);
        }
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if(sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            return new WidgetId(new Guid(value.ToString()));
        }
    }
}