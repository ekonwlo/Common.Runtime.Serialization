using System;
using System.Reflection;
using Common.Reflection;

namespace Common.Runtime.Serialization
{
    using Attributes;
    using Transformation;

    public abstract class Serializer<T>
        : BaseConverter, ISerializer<T>
    {
        private readonly PropertySetterDelegate _setter;
		private readonly PropertyGetterDelegate _getter;
        private readonly string _format;
        private Transformator _transformator;

        public virtual ISerializer<T> this[object item]
        {
            get { return this; }
        }

        //public SerializerFactory<T> Factory { get; private set; }
        public Type Type { get; private set; }
        public ISerializableProperty Attribute { get; private set; }
        public PropertyInfo Property { get; private set; }

        public override string Name
        {
            get { return Attribute.Name; }
        }

        public Transformator Transformator
        {
            get { return _transformator; }
            set { _transformator = value; }
        }
        
        internal Serializer(SerializerFactory<T> factory, Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
        {
            if (factory == null) throw new ArgumentNullException("factory", "Factory is required");
            if (type == null) throw new ArgumentNullException("type", "Type is required");
            if (property == null) throw new ArgumentNullException("property", "Property is required");
            if (attribute == null) throw new ArgumentNullException("attribute", "Attribute is required");

            Type = type;
            Property = property;
			Attribute = attribute;
            _setter = Property.CreateSetterDelegate();
			_getter = Property.CreateGetterDelegate();
            _format = format;
            _transformator = transformator;
        }

        private object GetPropertyRawValue(object item)
        {
            return item;
        }

        public virtual object GetPropertyValue(object item)
        {
			object value = _getter.Invoke(item);
            
            if (value == null && Attribute.Mandatory)
                throw new ArgumentNullException(Property.Name);

            if (_transformator != null)
                return _transformator.Transform(value);

            return value;
        }


        public override string GetPropertyString(object item)
        {
            object value = GetPropertyValue(item);

            if (value == null)
                return null;

            return value.ToString();
        }

        public virtual void SetPropertyValue(object item, object value)
        {
            if (value == null & !Attribute.Mandatory)
                return;

            if (_transformator != null)
            {
				_setter.Invoke(item, _transformator.Revert(value));
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

        public override object FromString(string text)
        {
            throw new NotImplementedException();
        }

    }
}
