using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Common.Runtime.Serialization.Json
{
    using Attributes;
    using Transformation;

    sealed class JPrimitiveSerializer
        : PrimitiveSerializer<JToken>
    {
        private MethodInfo _getGenericElementMethod;

        internal JPrimitiveSerializer(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
            : base(type, property, attribute, format, transformator ) 
        {
            MethodInfo getElementMethod = typeof(JPrimitiveSerializer).GetMethod("GetPrimitiveElement", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(JToken) }, null);
            _getGenericElementMethod = getElementMethod.MakeGenericMethod(new Type[] { type });
        }

        public override JToken ConvertFromObject(object item)
        {
            return (JToken)new JProperty(Attribute.Name, new JValue(GetPropertyValue(item)));
        }

        public override object ConvertToObject(JToken item)
        {
            if (item.Type == JTokenType.Null)
                return null;

            return _getGenericElementMethod.Invoke(this, new JToken[] { item });
            //return _getGenericElementMethod.Invoke(this, new JToken[] { item });
        }

        private T GetPrimitiveElement<T>(JToken item)
        {
            return (T) Convert.ChangeType(((JValue)item).Value, typeof(T));
        }

        public override object GetElementValue(JToken item)
        {
            return Convert.ChangeType(((JValue)item).Value, PrimitiveType);
        }

        public override JToken SetElementValue(object item)
        {
            return new JValue(item);
        }

    }
}
