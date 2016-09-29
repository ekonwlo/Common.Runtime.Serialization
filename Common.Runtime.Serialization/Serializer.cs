using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Common.Reflection;

namespace Common.Runtime.Serialization
{
    using Attributes;
    using Transformation;

    public abstract class Serializer<T>
        : BaseConverter, ISerializer<T>
    {

        //private ConstructorInfo _constructor;
        private readonly Type _type;
        private readonly PropertyInfo _property;
		private readonly PropertySetterDelegate _setter;
		private readonly PropertyGetterDelegate _getter;
        private readonly ISerializableProperty _attribute;
        private readonly string _format;
        private Transformator _transformator;

        public virtual ISerializer<T> this[object item]
        {
            get { return this; }
        }

        public Type Type
        {
            get { return _type; }
        }

        public override string Name
        {
            get { return _attribute.Name; }
        }

        public Transformator Transformator
        {
            get { return _transformator; }
            set { _transformator = value; }
        }
        
        internal PropertyInfo Property
        {
            get { return _property; }
        }

        public override ISerializableProperty Attribute
        {
            get { return _attribute; }
        }

        internal Serializer(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
        {
            _type = type;
            _property = property;
			_setter = property.CreateSetterDelegate();
			_getter = property.CreateGetterDelegate();
            _attribute = attribute;
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
            
            if (value == null && _attribute.Mandatory)
                throw new ArgumentNullException(_property.Name);

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
            if (value == null & !_attribute.Mandatory)
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
