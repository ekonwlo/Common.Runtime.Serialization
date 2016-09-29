using System;
using System.Collections.Generic;
using System.Reflection;
using Common.Reflection;

namespace Common.Runtime.Serialization
{
    using Attributes;
    using Transformation;

    public abstract class ObjectSerializer<T> 
        : Serializer<T>
    {
        private readonly IEnumerable<ISerializer<T>> _serializers;
        private readonly ConstructorDelegate _ctor;

		public IEnumerable<ISerializer<T>> Converters
        {
            get { return _serializers; }
        }

		protected ConstructorDelegate Constructor
		{
			get { return _ctor; }
		}

		//protected ObjectSerializer(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ConstructorInfo constructor)
		//	: base(type, property, attribute, format, transformator) 
		//{

		//	 SerializerFactory<T>.AddConverter(this);

		//   _ctor = constructor.CreateDelegate();
		//	// I can do this casting, becase those are internal classes used only in the namespace.
		//	_serializers = (Serializer<T>[]) SerializerFactory<T>.Create(type);
		//}

		protected ObjectSerializer(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ConstructorInfo constructor, IEnumerable<ISerializer<T>> serializers)
			: base(type, property, attribute, format, transformator)
		{

			SerializerFactory<T>.AddConverter(this);

			_ctor = constructor.CreateDelegate();
			// I can do this casting, becase those are internal classes used only in the namespace.
			_serializers =  serializers;
		}
    }
}
