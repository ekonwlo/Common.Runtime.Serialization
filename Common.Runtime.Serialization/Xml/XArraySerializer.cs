using System;
using System.Xml.Linq;
using System.Reflection;
using Common.Reflection;

namespace Common.Runtime.Serialization.Xml
{
    using Attributes;
    using Transformation;

    sealed class XArraySerializer
        : ArraySerializer<XObject>
    {
		internal XArraySerializer(SerializerFactory<XObject> factory, TypeDefinition type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ISerializer<XObject> elementSerializer)
            : base(factory, type, property, attribute, format, transformator, elementSerializer) 
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
