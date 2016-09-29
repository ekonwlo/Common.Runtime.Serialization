using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Runtime.Serialization.Attributes
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PropertyTransformationAttribute
        : System.Attribute
    {
      
        private readonly Type _type;
        private readonly string _dynamicExpression;
        private readonly Type[] _dynamicTypes;

        public Type Type
        {
            get { return _type; }
        }

        public string DynamicExpression
        {
            get { return _dynamicExpression; }
        }

        public Type[] DynamicTypes
        {
            get { return _dynamicTypes; }
        }

        public PropertyTransformationAttribute(Type type)
        {
            _type = type;
        }

        public PropertyTransformationAttribute(Type type, string dynamicExpression, params Type[] dynamicTypes)
        {
            _type = type;
            _dynamicExpression = dynamicExpression;
            _dynamicTypes = dynamicTypes;
        }

    }
}
