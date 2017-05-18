using System;
using System.Xml.Linq;
using System.Reflection;

namespace Common.Runtime.Serialization.Xml
{
    using Attributes;
    using Transformation;

    sealed class XPrimitiveSerializer
        : PrimitiveSerializer<XObject>
    {
        internal XPrimitiveSerializer(SerializerFactory<XObject> factory, Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
            : base(factory, type, property, attribute, format, transformator )
        { }


        public override object ConvertToObject(XObject item)
        {
            XElement element = item as XElement;

            if (element == null) throw new ArgumentException("Item must be XElement type", "item");

            if (element.Name != Attribute.Name) throw new ArgumentException(string.Format("Item is not a '{0}' element", Attribute.Name), "item");

            //SetPropertyString(null, element.Value);           
            return Convert.ChangeType(element.Value, Type);
        }

        public override XObject ConvertFromObject(object item)
        {
            return new XElement(Attribute.Name, new XText(GetPropertyString(item)));
        }        
    }
}
