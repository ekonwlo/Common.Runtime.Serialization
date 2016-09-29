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

    sealed class JArraySerializer
        : ArraySerializer<JToken>
    {
		internal JArraySerializer(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ISerializer<JToken> serializer)
            : base(type, property, attribute, format, transformator, serializer) 
        { }

        public override JToken ConvertFromObject(object item)
        {
            JToken content = (JToken) SetElementsMethod(1).Invoke(this, new object[] { GetPropertyValue(item), 1 });
            JToken token = new JProperty(Name, content);

            return token;
        }

        protected override JToken SetElements<T>(T[] items, int dimension)
        {

            if (Dimiensions == dimension)
            {
                return new JArray((from item in items
                                   select Converter.SetElementValue(item)).ToArray());
            }

            JToken array = new JArray((from item in items
                                       select (JToken) SetElementsMethod(dimension + 1).Invoke(this, new object[] { item, dimension + 1 })).ToArray());
            return array;
        }

		public override object ConvertToObject(JToken item)
        {
            if (item == null)
                return null;

            object elements = (object) GetElementsMethod(1).Invoke(this, new object[] { item, 1 });

            return elements;
        }

        protected override T[] GetElements<T>(JToken item, int dimension)
        {

            if (Dimiensions == dimension)
            {
                T[] values = (from JToken child in item.Children()
                              select (T) Converter.GetElementValue(child)).ToArray();
                return (T[]) values;
            }

            object elements = (from JToken child in item.Children()
                               select (T) GetElementsMethod(dimension + 1).Invoke(this, new object[] { child, dimension + 1 })).ToArray();

            return (T[])elements;
        }

        public sealed override object FromString(string text)
        {
            return ConvertToObject(JToken.Parse(text));
        }
    }
}
