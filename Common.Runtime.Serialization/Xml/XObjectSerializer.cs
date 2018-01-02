using System;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;
using Common.Reflection;
using Common.Runtime.Serialization.Parsers;

namespace Common.Runtime.Serialization.Xml
{
    using Attributes;
    using Transformation;

    sealed class XObjectSerializer
        : ObjectSerializer<XObject>
    {
		internal XObjectSerializer(SerializerFactory<XObject> factory
            , TypeDefinition type
            , PropertyInfo property
            , ISerializableProperty attribute
            , string format
            , Transformator transformator
            , ConstructorInfo constructor
            , SerializerFactory<XObject>.CreateSerializersDelegate<ISerializableProperty> createSerializersCallback)
            : base(factory, type, property, attribute, format, transformator, constructor, createSerializersCallback)
        { }

        public override XObject ConvertFromObject(object item)
        {
            object value = GetPropertyValue(item);

            XElement element = new XElement(Name, (from ISerializer<XObject> serializer in Serializers
                                                   select serializer.ConvertFromObject(value)).ToArray());

            return element;
        }

        public override object ConvertToObject(XObject item)
        {
            if (item == null)
                return null;

            XElement element = item as XElement;
            if (element == null) throw new ArgumentException("Item must be XElement type", "item");

            object o = Constructor.Invoke();

            foreach (ISerializer<XObject> serializer in Serializers)
            {
                XElement child = element.Element(serializer.Name);

                if (child == null)
                    serializer.SetPropertyValue(o, null);
                else
                {
                    serializer.SetPropertyValue(o, serializer[o].ConvertToObject(child));
                }
            }
            
            return o;
        }

        protected sealed override object From<U>(IParser<XObject, U> parser, U input)
        {
            return ConvertToObject(parser.ParseFrom(input));
        }

    }
}
