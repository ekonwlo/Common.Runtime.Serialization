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
		public IEnumerable<ISerializer<T>> Serializers { get; private set; }
		protected ConstructorDelegate Constructor { get; private set; }
        
		protected ObjectSerializer(SerializerFactory<T> factory
            , Type type
            , PropertyInfo property
            , ISerializableProperty attribute
            , string format, Transformator transformator
            , ConstructorInfo constructor
            , SerializerFactory<T>.CreateSerializersDelegate<ISerializableProperty> createSerializersCallback)
            : base(factory, type, property, attribute, format, transformator)
        {
            Constructor = constructor.CreateDelegate();

            factory.Add(this);
            Serializers = createSerializersCallback(type);            
        }
    }
}
