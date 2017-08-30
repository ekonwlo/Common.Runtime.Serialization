using System;
using System.Reflection;
using Common.Reflection.MultiTarget;

namespace Common.Runtime.Serialization
{
    using Attributes;
    using Transformation;

    public abstract class PrimitiveSerializer<T>
         : Serializer<T>
    {
        protected PrimitiveSerializer(SerializerFactory<T> factory, Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
            : base(factory, type, property, attribute, format, transformator) 
        {
            if (!type.IsPrimitive() && typeof(string) != type)
            {
                throw new ArgumentException("Type is neither string nor primitive type", "type");
            }
		}

        protected U GetValue<U>(object value)
        {

            return default(U);
        }


    }
}
