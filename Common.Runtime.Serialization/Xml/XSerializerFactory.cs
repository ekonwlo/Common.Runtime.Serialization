using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Reflection;
using Common.Reflection;

namespace Common.Runtime.Serialization.Xml
{
    using Attributes;
    using Transformation;
    using Parsers;

    public sealed class XSerializerFactory
        : SerializerFactory<XObject>
    {
        public XSerializerFactory() : base()
        {
            Parsers.Register<string>(new XStringParser());
        }

        public override ISerializer[] Create<U>(Type type)
        {
            return CreateSerializers<U>(type);
        }

        protected override ISerializer<XObject> CreatePrimitiveConverter(TypeDefinition type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
        {
            return new XPrimitiveSerializer(this, type, property, attribute, format, transformator);
        }

        protected override ISerializer<XObject> CreateArrayConverter(TypeDefinition type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ISerializer<XObject> serializer)
        {
            return new XArraySerializer(this, type, property, attribute, format, transformator, serializer);
        }

        protected override ISerializer<XObject> CreateObjectConverter(TypeDefinition type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ConstructorInfo constructor, CreateSerializersDelegate<ISerializableProperty> createSerializersCallback)
        {
            return new XObjectSerializer(this, type, property, attribute, format, transformator, constructor, createSerializersCallback);
        }

        protected override ISerializer<XObject> CreateDynamicConverter(TypeDefinition type, PropertyInfo property, ISerializableProperty attribute, string format, TransofmationSelector selector, IDictionary<Type, ISerializer<XObject>> serializers)
        {
            return new XDynamicSerializer(this, type, property, attribute, format, selector, serializers);
        }

        protected override ISerializer<XObject> CreateNullableConverter(TypeDefinition type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
        {
            return new XNullableSerializer(this, type, property, attribute, format, transformator);
        }
    }
}
