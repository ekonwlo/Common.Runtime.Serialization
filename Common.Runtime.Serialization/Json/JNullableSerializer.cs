using System;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Common.Reflection;

namespace Common.Runtime.Serialization.Json
{
    using Attributes;
    using Transformation;

    sealed class JNullableSerializer
        : NullableSerializer<JToken>
    {
        internal JNullableSerializer(SerializerFactory<JToken> factory, TypeDefinition type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
            : base(factory, type, property, attribute, format, transformator)
        { }

        public override JToken ConvertFromObject(object item)
        {
            throw new NotImplementedException();
        }

        public override object ConvertToObject(JToken item)
        {
            throw new NotImplementedException();
        }

        public override object GetElementValue(JToken item)
        {
            if (item.Type == JTokenType.Null)
                return null;

            return Convert.ChangeType(((JValue)item).Value, UnderlyingType);
        }

        public override JToken SetElementValue(object item)
        {
            if (item == null)
                return null;

            return new JValue(item);
        }

    }
}
