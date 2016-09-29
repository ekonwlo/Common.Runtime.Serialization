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

    sealed class JDynamicSerializer
        : DynamicSerializer<JToken>
    {
		internal JDynamicSerializer(Type type, PropertyInfo property, ISerializableProperty attribute, string format, TransofmationSelector selector, IDictionary<Type, ISerializer<JToken>> serializers)
            : base(type, property, attribute, format, selector, serializers) 
        { }
        
    }
}
