using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Reflection;
using Common.Reflection;

namespace Common.Runtime.Serialization.Xml
{
    using Attributes;
    using Transformation;

    sealed class XDynamicSerializer
        : DynamicSerializer<XObject>
    {
		internal XDynamicSerializer(SerializerFactory<XObject> factory, TypeDefinition type, PropertyInfo property, ISerializableProperty attribute, string format, TransofmationSelector selector, IDictionary<Type, ISerializer<XObject>> serializers)
            : base(factory, type, property, attribute, format, selector, serializers)
        { }   

    }
}
