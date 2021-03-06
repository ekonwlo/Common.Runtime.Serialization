﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Common.Reflection;

namespace Common.Runtime.Serialization.Json
{
    using Attributes;
    using Transformation;

    sealed class JDynamicSerializer
        : DynamicSerializer<JToken>
    {
		internal JDynamicSerializer(SerializerFactory<JToken> factory, TypeDefinition type, PropertyInfo property, ISerializableProperty attribute, string format, TransofmationSelector selector, IDictionary<Type, ISerializer<JToken>> serializers)
            : base(factory, type, property, attribute, format, selector, serializers) 
        { }
        
    }
}
