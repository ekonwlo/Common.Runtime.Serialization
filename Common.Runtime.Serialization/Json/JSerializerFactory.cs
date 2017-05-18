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
            return CreateSerializsers<U>(type);
        }

		protected override ISerializer<JToken> CreatePrimitiveConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
        {
            return new JPrimitiveSerializer(this, type, property, attribute, format, transformator);
        }

		public override ISerializer<JToken> CreateArrayConverter(Type baseType, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ISerializer<JToken> serializer)
        {
            return new JArraySerializer(this, baseType, property, attribute, format, transformator, serializer);
        }
        
        protected override ISerializer<JToken> CreateObjectConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ConstructorInfo constructor, CreateSerializersDelegate<ISerializableProperty> createSerializersCallback)
        {
            return new JObjectSerializer(this, type, property, attribute, format, transformator, constructor, createSerializersCallback);
        }

        protected override ISerializer<JToken> CreateDynamicConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, TransofmationSelector selector, IDictionary<Type, ISerializer<JToken>> serializers)
        {
            return new JDynamicSerializer(this, type, property, attribute, format, selector, serializers);
        }

		protected override ISerializer<JToken> CreateNullableConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
        {
            return new JNullableSerializer(this, type, property, attribute, format, transformator);
        }
    }
}
