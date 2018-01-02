using System;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using Common.Reflection;
using Common.Runtime.Serialization.Parsers;

namespace Common.Runtime.Serialization.Xml
{
    using Attributes;
    using Transformation;

    sealed class XPrimitiveSerializer
        : PrimitiveSerializer<XObject>
    {
        internal XPrimitiveSerializer(SerializerFactory<XObject> factory, TypeDefinition type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
            : base(factory, type, property, attribute, format, transformator)
        { }

        public override object ConvertToObject(XObject item)
        {
            XElement element = item as XElement;

            if (element == null) throw new ArgumentException("Item must be XElement type", "item");

            if (element.Name != Attribute.Name) throw new ArgumentException(string.Format("Item is not a '{0}' element", Attribute.Name), "item");

            return Convert.ChangeType(element.Value, Type);
        }

        public override XObject ConvertFromObject(object item)
        {
            object value = GetPropertyValue(item);

            if (value == null)
            {
                return new XElement(Attribute.Name);
            }
            else
            {
                return new XElement(Attribute.Name, new XText(value.ToString()));
            }
        }

        protected override U To<U>(IParser<XObject, U> parser, object item)
        {
            XObject token;

            if (item == null)
            {
                return default(U);
            }
            else if (Type.Equals(TypeDefinition.Of(item.GetType())))
            {
                token = new XText(item.ToString());
            }
            else
            {
                throw new SerializationException(string.Format("{0} serializer  cannot parse item of type {1}", Type.Name, item.GetType().Name));
            }

            return parser.ParseTo(token);
        }

        protected override object From<U>(IParser<XObject, U> parser, U input)
        {
            XObject token = parser.ParseFrom(input);

            switch (token.NodeType)
            {
                case XmlNodeType.Text:
                    try
                        {
                            return Convert.ChangeType(input, Type);
                        }
                        catch (FormatException ex)
                        {
                            throw new SerializationException(string.Format("{0} serializer cannot parse item {1}", Type.Name, input), ex);
                        }
                        catch (OverflowException ex)
                        {
                            throw new SerializationException(string.Format("{0} serializer cannot parse item {1}", Type.Name, input), ex);
                        }
                default:
                    throw new SerializationException(string.Format("{0} serializer cannot parse item from type {1}", Type.Name, token.NodeType));
            }
            
        }


    }
}
