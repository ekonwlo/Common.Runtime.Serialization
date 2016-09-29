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

    sealed class JObjectSerializer
        : ObjectSerializer<JToken>
    {
        internal JObjectSerializer(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ConstructorInfo constructor, IEnumerable<ISerializer<JToken>> serializers)
            : base(type, property, attribute, format, transformator, constructor, serializers) 
        { }

        public override JToken ConvertFromObject(object item)
        {
            JToken content = new JObject((from Serializer<JToken> converter in Converters
                                          select converter.ConvertFromObject(GetPropertyValue(item))).ToArray());

            JToken token = new JProperty(Name, content);

            return token;
        }

        public override object ConvertToObject(JToken item)
        {
            return GetElementValue(item);

            //object subitem = Constructor.Invoke(null);

            //foreach (TypeConverter<JToken> serializer in Converters)
            //{ 

            //    JToken child = item[serializer.Name];

            //    if (child == null)
            //        serializer.SetPropertyValue(subitem, null);
            //    else
            //        serializer.SetPropertyValue(subitem, serializer.ConvertToObject(child));
            //}


            //return subitem;
        }

        public override object GetElementValue(JToken item)
        {
            object subitem = Constructor.Invoke();

            foreach (ISerializer<JToken> converter in Converters)
            {
                JToken child = item[converter.Name];

                if (child == null)
                    converter.SetPropertyValue(subitem, null);
                else
                {
                    converter.SetPropertyValue(subitem, converter[subitem].ConvertToObject(child));
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
