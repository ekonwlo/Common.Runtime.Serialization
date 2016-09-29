using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

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

        protected NullableSerializer(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
            : base(type, property, attribute, format, transformator) 
        {
            _type = Nullable.GetUnderlyingType(type);
        }
    }
}
