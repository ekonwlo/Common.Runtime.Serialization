using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Common.Runtime.Serialization.Json
{
    using Attributes;
    using Transformation;

    sealed class JObjectSerializer
        : ObjectSerializer<JToken>
    {
        internal JObjectSerializer(SerializerFactory<JToken> factory
            , Type type
            , PropertyInfo property
            , ISerializableProperty attribute
            , string format
            , Transformator transformator
            , ConstructorInfo constructor
            , SerializerFactory<JToken>.CreateSerializersDelegate<ISerializableProperty> createSerializersCallback)
            : base(factory, type, property, attribute, format, transformator, constructor, createSerializersCallback)
        { }

        public override JToken ConvertFromObject(object item)
        {
            JToken content = new JObject((from Serializer<JToken> converter in Serializers
                                          select converter.ConvertFromObject(GetPropertyValue(item))).ToArray());

            JToken token = new JProperty(Name, content);

            return token;
        }

        public override object ConvertToObject(JToken item)
        {
            if(item == null)
            {
                if(Attribute.Mandatory) throw new ArgumentNullException("item", "Item is mandatory");
                return null;
            }
            
            switch (item.Type)
            {
                case JTokenType.Property:
                    return ConvertToObject(item as JProperty);
                case JTokenType.Object:
                    return ConvertToObject(item as JObject);
                default:
                    throw new ArgumentException("Not supported JToken type", "item");
            }
        }

        private object ConvertToObject(JProperty item)
        {
            if (item.Name != Attribute.Name) throw new ArgumentException(string.Format("Item is not a '{0}' element", Attribute.Name), "item");

            return ConvertToObject(item.Value as JObject);
        }

        private object ConvertToObject(JObject item)
        {
            object subitem = Constructor.Invoke();

            foreach (ISerializer<JToken> serializer in Serializers)
            {
                JToken child = item[serializer.Name];

                if (child == null)
                    serializer.SetPropertyValue(subitem, null);
                else
                {
                    serializer.SetPropertyValue(subitem, serializer[subitem].ConvertToObject(child));
                }
            }
            return subitem;
        }
        
        public override JToken SetElementValue(object item)
        {         
            return base.SetElementValue(item);
        }

        public sealed override string ToString(object item)
        {
            return ConvertFromObject(item).First.ToString();
        }

        public sealed override object FromString(string text)
        {
            return ConvertToObject(JToken.Parse(text));
        }

    }
}
