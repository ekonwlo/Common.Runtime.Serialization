using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Linq;

namespace Common.Runtime.Serialization.Xml
{
    using Attributes;
    using Transformation;

    sealed class XNullableSerializer
        : NullableSerializer<XObject>
    {
        internal XNullableSerializer(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
            : base(type, property, attribute, format, transformator)
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
