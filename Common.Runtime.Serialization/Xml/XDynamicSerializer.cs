using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Linq;

namespace Common.Runtime.Serialization.Xml
{
    using Attributes;
    using Transformation;

    sealed class XDynamicSerializer
        : DynamicSerializer<XObject>
    {
		internal XDynamicSerializer(SerializerFactory<XObject> factory, Type type, PropertyInfo property, ISerializableProperty attribute, string format, TransofmationSelector selector, IDictionary<Type, ISerializer<XObject>> serializers)
            : base(factory, type, property, attribute, format, selector, serializers)
        { }   

    }
}
