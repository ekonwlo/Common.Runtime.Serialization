using System;
using System.Reflection;
using Common.Reflection;

namespace Common.Runtime.Serialization
{
    using Attributes;
    using Transformation;

    public abstract class NullableSerializer<T>
        : Serializer<T>
    {
        private readonly Type _type;

        public Type UnderlyingType
        {
            get { return _type; }
        }

        protected NullableSerializer(SerializerFactory<T> factory, TypeDefinition type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
            : base(factory, type, property, attribute, format, transformator) 
        {
            _type = Nullable.GetUnderlyingType(type);
        }
    }
}
