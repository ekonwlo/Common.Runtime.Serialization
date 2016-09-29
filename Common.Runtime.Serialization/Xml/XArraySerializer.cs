using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;

namespace Common.Runtime.Serialization.Xml
{
    using Attributes;
    using Transformation;

    sealed class XArraySerializer
        : ArraySerializer<XObject>
    {
		internal XArraySerializer(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ISerializer<XObject> serializer)
            : base(type, property, attribute, format, transformator, serializer) 
        { }   

        public override XObject ConvertFromObject(object item)
        {
            throw new NotImplementedException();
        }

        public override object ConvertToObject(XObject item)
        {
            throw new NotImplementedException();
        }   
    }
}
