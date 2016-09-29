using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Common.Runtime.Serialization
{
    using Attributes;
    using Transformation;

    public abstract class PrimitiveSerializer<T>
         : Serializer<T>
    {

        private readonly Type _type;
        //private MethodInfo _getGenericElementMethod;

        public Type PrimitiveType
        {
            get { return _type; }
        }

        protected PrimitiveSerializer(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
            : base(type, property, attribute, format, transformator) 
        {
            _type = type;

			//MethodInfo getElementMethod = typeof(PrimitiveSerializer<T>).GetMethod("GetValue", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(object) }, null);
			//_getGenericElementMethod = getElementMethod.MakeGenericMethod(new Type[] { type });
        }

        protected U GetValue<U>(object value)
        {

            return default(U);
        }


    }
}
