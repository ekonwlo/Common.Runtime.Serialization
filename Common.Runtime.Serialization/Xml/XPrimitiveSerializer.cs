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

    sealed class XPrimitiveSerializer
        : PrimitiveSerializer<XObject>
    {
        internal XPrimitiveSerializer(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
            : base(type, property, attribute, format, transformator ) { }


        public override XObject ConvertFromObject(object item)
        {
            return new XElement(Attribute.Name, new XText(GetPropertyString(item)));
        }

        public override object ConvertToObject(XObject item)
        {
            throw new NotImplementedException();
        }
    }
}
