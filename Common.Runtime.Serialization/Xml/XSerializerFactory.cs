using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace Common.Runtime.Serialization.Xml
{
    using Attributes;
    using Transformation;

    public sealed class XSerializerFactory
        : SerializerFactory<XObject>
    {
		public override ISerializer<XObject>[] Create<U>(Type type)
        {
            return CreateConverters<U>(type);
        }

		protected override ISerializer<XObject> CreatePrimitiveConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
        {
            return new XPrimitiveSerializer(type, property, attribute, format, transformator);
        }

		public override ISerializer<XObject> CreateArrayConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ISerializer<XObject> serializer)
        {
            return new XArraySerializer(type, property, attribute, format, transformator, serializer);
        }

		public override ISerializer<XObject> CreateObjectConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ConstructorInfo constructor, IEnumerable<ISerializer<XObject>> serializers)
        {
            return new XObjectSerializer(type, property, attribute, format, transformator, constructor, serializers);
        }

		protected override ISerializer<XObject> CreateDynamicConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, TransofmationSelector selector, IDictionary<Type, ISerializer<XObject>> serializers)
        {
            return new XDynamicSerializer(type, property, attribute, format, selector, serializers);
        }

		protected override ISerializer<XObject> CreateNullableConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
        {
            return new XNullableSerializer(type, property, attribute, format, transformator);
        }
    }
}
