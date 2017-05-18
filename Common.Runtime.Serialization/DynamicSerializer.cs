using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Common.Runtime.Serialization
{
    using Attributes;
    using Transformation;

    public abstract class DynamicSerializer<T>
        : Serializer<T>
    {
        private readonly TransofmationSelector _selector;
        private readonly IDictionary<Type, ISerializer<T>> _serializers;

        public override ISerializer<T> this[object item]
        {
            get
            {
                if (_selector == null)
                    throw new ArgumentNullException("Dynamic serializer requires transformation selector.");

                Type type = _selector[item];

                return _serializers[type];
            }
        }

        protected DynamicSerializer(SerializerFactory<T> factory, Type type, PropertyInfo property, ISerializableProperty attribute, string format, TransofmationSelector selector, IDictionary<Type, ISerializer<T>> serializers)
            : base(factory, type, property, attribute, format, null)
        {
	        _selector = selector;
			_serializers = serializers;
            
        }

        public override object ConvertToObject(T item)
        {
            throw new NotImplementedException();
        }

        public override T ConvertFromObject(object item)
        {
            throw new NotImplementedException();
        }

        public override void SetPropertyValue(object item, object value)
        {
            if (value == null)
            {
                base.SetPropertyValue(item, value);
                return;
            }
            _serializers[value.GetType()].SetPropertyValue(item, value);
        }
    }
}
