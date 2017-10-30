using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Common.Reflection;

namespace Common.Runtime.Serialization.Json
{
    using Attributes;
    using Transformation;

    public sealed class JSerializerFactory
        : SerializerFactory<JToken>
    {

        public override ISerializer[] Create<U>(Type type)
        {
            return CreateSerializers<U>(type);
        }

        protected override ISerializer<JToken> CreatePrimitiveConverter(TypeDefinition type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
        {
            return new JPrimitiveSerializer(this, type, property, attribute, format, transformator);
        }

        protected override ISerializer<JToken> CreateArrayConverter(TypeDefinition baseType, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ISerializer<JToken> serializer)
        {
            return new JArraySerializer(this, baseType, property, attribute, format, transformator, serializer);
        }

        protected override ISerializer<JToken> CreateObjectConverter(TypeDefinition type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ConstructorInfo constructor, CreateSerializersDelegate<ISerializableProperty> createSerializersCallback)
        {
            return new JObjectSerializer(this, type, property, attribute, format, transformator, constructor, createSerializersCallback);
        }

        protected override ISerializer<JToken> CreateDynamicConverter(TypeDefinition type, PropertyInfo property, ISerializableProperty attribute, string format, TransofmationSelector selector, IDictionary<Type, ISerializer<JToken>> serializers)
        {
            return new JDynamicSerializer(this, type, property, attribute, format, selector, serializers);
        }

        protected override ISerializer<JToken> CreateNullableConverter(TypeDefinition type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
        {
            return new JNullableSerializer(this, type, property, attribute, format, transformator);
        }
    }
}
