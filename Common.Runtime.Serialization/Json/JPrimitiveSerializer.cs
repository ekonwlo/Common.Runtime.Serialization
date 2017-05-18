using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Common.Runtime.Serialization.Json
{
    using Attributes;
    using Transformation;

    sealed class JPrimitiveSerializer
        : PrimitiveSerializer<JToken>
    {
        internal JPrimitiveSerializer(SerializerFactory<JToken> factory, Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
            : base(factory, type, property, attribute, format, transformator ) 
        { }

        public override JToken ConvertFromObject(object item)
        {
            return new JProperty(Attribute.Name, new JValue(GetPropertyValue(item)));
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
                    return ConvertToObject((JValue)item); //TODO: switch case all types
            }
        }
        
        private object ConvertToObject(JProperty item)
        {
            if (item.Name != Attribute.Name) throw new ArgumentException(string.Format("Item is not a '{0}' element", Attribute.Name), "item");

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

    }
}
