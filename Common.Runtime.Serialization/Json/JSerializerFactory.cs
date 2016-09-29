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

    public sealed class JSerializerFactory
        : SerializerFactory<JToken>
    {

		public override ISerializer<JToken>[] Create<U>(Type type)
        {
            return CreateConverters<U>(type);
        }

		protected override ISerializer<JToken> CreatePrimitiveConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
        {
            return new JPrimitiveSerializer(type, property, attribute, format, transformator);
        }

		public override ISerializer<JToken> CreateArrayConverter(Type baseType, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ISerializer<JToken> serializer)
        {
            return new JArraySerializer(baseType, property, attribute, format, transformator, serializer);
        }

		public override ISerializer<JToken> CreateObjectConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ConstructorInfo constructor, IEnumerable<ISerializer<JToken>> serializers)
        {
            return new JObjectSerializer(type, property, attribute, format, transformator, constructor, serializers);
        }

		protected override ISerializer<JToken> CreateDynamicConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, TransofmationSelector selector, IDictionary<Type, ISerializer<JToken>> serializers)
        {
            return new JDynamicSerializer(type, property, attribute, format, selector, serializers);
        }

		protected override ISerializer<JToken> CreateNullableConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
        {
            return new JNullableSerializer(type, property, attribute, format, transformator);
        }
    }
}
