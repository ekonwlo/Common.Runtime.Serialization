using System;
using System.Reflection;
using Common.Reflection;

namespace Common.Runtime.Serialization
{
    using Attributes;
    using Transformation;

    public abstract class Serializer<T>
        : BaseSerializer, ISerializer<T>
    {
        private readonly PropertySetterDelegate _setter;
        private readonly PropertyGetterDelegate _getter;
        private readonly string _format;

        public virtual ISerializer<T> this[object item]
        {
            get { return this; }
        }

        public TypeDefinition Type { get; private set; }
        public PropertyInfo Property { get; private set; }

        internal Serializer(SerializerFactory<T> factory, TypeDefinition type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
            : base(attribute, transformator)
        {
            if (factory == null) throw new ArgumentNullException("factory", "Factory is required");
            if (type == null) throw new ArgumentNullException("type", "Type is required");
            if (property == null) throw new ArgumentNullException("property", "Property is required");

            Type = type;
            Property = property;

            _setter = Property.CreateSetterDelegate();
            _getter = Property.CreateGetterDelegate();
            _format = format;
        }

        private object GetPropertyRawValue(object item)
        {
            return item;
        }

        public override object GetPropertyValue(object item)
        {
            object value = _getter.Invoke(item);

            if (value == null && Attribute.Mandatory)
                throw new ArgumentNullException(Property.Name);

            if (Transformator != null)
                return Transformator.Transform(value);

            return value;
        }

        public override string GetPropertyString(object item)
        {
            object value = GetPropertyValue(item);

            if (value == null)
                return null;

            return value.ToString();
        }

        public override void SetPropertyValue(object item, object value)
        {
            if (value == null & !Attribute.Mandatory)
                return;

            if (Transformator != null)
            {
                _setter.Invoke(item, Transformator.Revert(value));
                return;
            }
            _setter.Invoke(item, value);
        }

        public override void SetPropertyString(object item, string value)
        {
            SetPropertyValue(item, FromString(value));
        }

        public abstract T ConvertFromObject(object item);
        public abstract object ConvertToObject(T item);

        public virtual T SetElementValue(object item)
        {
            throw new NotImplementedException();
        }

        public virtual object GetElementValue(T item)
        {
            return null;
        }

        public override string ToString(object item)
        {
            return ConvertFromObject(item).ToString();
        }

        public override object FromString(string value)
        {
            throw new NotImplementedException();
        }

    }
}
