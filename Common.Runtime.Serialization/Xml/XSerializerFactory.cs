using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace Common.Runtime.Serialization.Xml
{
    using Attributes;
    using Transformation;

    public sealed class XSerializerFactory
        : SerializerFactory<XObject>
    {
		public override ISerializer<XObject>[] Create<U>(Type type)
        {
            return CreateSerializsers<U>(type);
        }

		protected override ISerializer<XObject> CreatePrimitiveConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
        {
            return new XPrimitiveSerializer(this, type, property, attribute, format, transformator);
        }

		public override ISerializer<XObject> CreateArrayConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ISerializer<XObject> serializer)
        {
            return new XArraySerializer(this, type, property, attribute, format, transformator, serializer);
        }
        
        protected override ISerializer<XObject> CreateObjectConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ConstructorInfo constructor, CreateSerializersDelegate<ISerializableProperty> createSerializersCallback)
        {
            return new XObjectSerializer(this, type, property, attribute, format, transformator, constructor, createSerializersCallback);
        }

        protected override ISerializer<XObject> CreateDynamicConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, TransofmationSelector selector, IDictionary<Type, ISerializer<XObject>> serializers)
        {
            return new XDynamicSerializer(this, type, property, attribute, format, selector, serializers);
        }

		protected override ISerializer<XObject> CreateNullableConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
        {
            return new XNullableSerializer(this, type, property, attribute, format, transformator);
        }
    }
}
