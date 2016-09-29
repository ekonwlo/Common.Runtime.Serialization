using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;

namespace Common.Runtime.Serialization.Xml
{
    using Attributes;
    using Transformation;

    sealed class XObjectSerializer
        : ObjectSerializer<XObject>
    {
		internal XObjectSerializer(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ConstructorInfo constructor, IEnumerable<ISerializer<XObject>> serializers)
            : base(type, property, attribute, format, transformator, constructor, serializers) 
        { }  

        public override XObject ConvertFromObject(object item)
        {

            XElement element = new XElement(Name, (from Serializer<XObject> converter in Converters
                                                   select converter.ConvertFromObject(GetPropertyValue(item))).ToArray());

            return element;
        }

        public override object ConvertToObject(XObject item)
        {
            throw new NotImplementedException();
        }

        public sealed override object FromString(string text)
        {
            return ConvertToObject(XElement.Parse(text));
        }

    }
}
