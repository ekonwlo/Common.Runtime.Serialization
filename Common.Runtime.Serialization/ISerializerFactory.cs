﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace Common.Runtime.Serialization
{
	using Attributes;
	using Transformation;

	public interface ISerializerFactory<T>
	{
		ISerializer<T> CreateArrayConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ISerializer<T> serializer); //where U : RestAttribute;
		ISerializer<T> CreateObjectConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ConstructorInfo constructor, IEnumerable<ISerializer<T>> serializers); //where U : RestAttribute;
        
	}
}