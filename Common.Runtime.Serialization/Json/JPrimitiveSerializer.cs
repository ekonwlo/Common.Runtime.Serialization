using System;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Common.Reflection;
using Common.Runtime.Serialization.Parsers;

namespace Common.Runtime.Serialization.Json
{
    using Attributes;
    using Transformation;


    sealed class JPrimitiveSerializer
        : PrimitiveSerializer<JToken>
    {
        internal JPrimitiveSerializer(SerializerFactory<JToken> factory, TypeDefinition type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
            : base(factory, type, property, attribute, format, transformator)
        { }

        public override JToken ConvertFromObject(object item)
        {
            if (item == null)
            {
                return JValue.CreateNull();
            }
            else if (Type.Equals(TypeDefinition.Of(item.GetType())))
            {
                return new JValue(item);
            }
            else
            {
                return new JProperty(Attribute.Name, new JValue(GetPropertyValue(item)));
            }
        }

        public override object ConvertToObject(JToken item)
        {
            if (item == null)
            {
                if (Attribute.Mandatory) throw new ArgumentNullException("item", "Item is mandatory");
                return null;
            }

            switch (item.Type)
            {
                case JTokenType.Null:
                    return null;
                case JTokenType.Property:
                    return ConvertToObject(item as JProperty);
                default:
                    return ConvertToObject(item as JValue); //TODO: switch case all types
            }
        }

        private object ConvertToObject(JProperty item)
        {
            if (item.Name != Attribute.Name)
                throw new ArgumentException(string.Format("Item is not a '{0}' element", Attribute.Name), "item");

            return ConvertToObject(item.Value as JValue);
        }

        private object ConvertToObject(JValue item)
        {
            return Convert.ChangeType(item.Value, Type);
        }

        public override object GetElementValue(JToken item)
        {
            return Convert.ChangeType(((JValue)item).Value, Type);
        }

        public override JToken SetElementValue(object item)
        {
            return new JValue(item);
        }

        protected override U To<U>(IParser<JToken, U> parser, object item)
        {
            JToken token;

            if (item == null)
            {
                token = JValue.CreateNull();
            }
            else if (Type.Equals(TypeDefinition.Of(item.GetType())))
            {
                token = new JValue(item);
            }
            else
            {
                throw new SerializationException(string.Format("{0} serializer  cannot parse item of type {1}", Type.Name, item.GetType().Name));
            }

            return parser.ParseTo(token);
        }

        protected override object From<U>(IParser<JToken, U> parser, U input)
        {
            JToken token = parser.ParseFrom(input);

            switch (token.Type)
            {
                case JTokenType.Null:
                    return null;
                case JTokenType.Boolean:
                case JTokenType.Integer:
                case JTokenType.Float:
                case JTokenType.String:
                    try
                    {
                        return Convert.ChangeType((token as JValue).Value, Type);
                    }
                    catch (FormatException ex)
                    {
                        throw new SerializationException(string.Format("{0} serializer cannot parse item {1}", Type.Name, token), ex);
                    }
                    catch (OverflowException ex)
                    {
                        throw new SerializationException(string.Format("{0} serializer cannot parse item {1}", Type.Name, token), ex);
                    }
                default:
                    throw new SerializationException(string.Format("{0} serializer cannot parse item from type {1}", Type.Name, token.Type));
            }
        }
    }
}
