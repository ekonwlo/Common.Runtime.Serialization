﻿using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Common.Reflection;
using Common.Runtime.Serialization.Parsers;

namespace Common.Runtime.Serialization.Json
{
    using Attributes;
    using Transformation;

    sealed class JArraySerializer
        : ArraySerializer<JToken>
    {
        internal JArraySerializer(SerializerFactory<JToken> factory, TypeDefinition type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ISerializer<JToken> elementSerializer)
            : base(factory, type, property, attribute, format, transformator, elementSerializer)
        { }

        public override JToken ConvertFromObject(object item)
        {
            if (item == null)
            {
                if (Attribute.Mandatory) throw new ArgumentNullException("item", "Item is mandatory");
                return JValue.CreateNull();
            }

            JToken array;
            if (item.GetType().IsArray)
            {
                array = (JToken)SetElementsMethod(1).Invoke(this, new object[] { item, 1 });
                return array;
            }

            array = (JToken)SetElementsMethod(1).Invoke(this, new object[] { GetPropertyValue(item), 1 });
            JToken token = new JProperty(Name, array);

            return token;
        }

        protected override JToken SetElements<T>(T[] items, int dimension)
        {

            if (Dimiensions == dimension)
            {
                return new JArray((from item in items
                                   select ElementSerializer.SetElementValue(item)).ToArray());
            }

            JToken array = new JArray((from item in items
                                       select (JToken)SetElementsMethod(dimension + 1).Invoke(this, new object[] { item, dimension + 1 })).ToArray());
            return array;
        }

        public override object ConvertToObject(JToken item)
        {
            if (item == null)
                return null;

            object elements = GetElementsMethod(1).Invoke(this, new object[] { item, 1 });

            return elements;
        }

        protected override T[] GetElements<T>(JToken item, int dimension)
        {

            if (Dimiensions == dimension)
            {
                T[] values = (from JToken child in item.Children()
                              select (T)ElementSerializer.GetElementValue(child)).ToArray();
                return values;
            }

            T[] elements = (from JToken child in item.Children()
                            select (T)GetElementsMethod(dimension + 1).Invoke(this, new object[] { child, dimension + 1 })).ToArray();

            return elements;
        }

        protected sealed override object From<U>(IParser<JToken, U> parser, U input)
        {
            JToken token = parser.ParseFrom(input);

            switch (token.Type)
            {
                case JTokenType.Null:
                    return null;
                case JTokenType.Array:
                    return GetElementsMethod(1).Invoke(this, new object[] { token, 1 });
                default:
                    throw new SerializationException(string.Format("{0} serializer cannot parse item from type {1}", Type.Name, token.Type));
            }
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
                token = (JToken)SetElementsMethod(1).Invoke(this, new object[] { item, 1 });
            }
            else
            {
                throw new SerializationException(string.Format("{0} serializer  cannot parse item of type {1}", Type.Name, item.GetType().Name));
            }

            return parser.ParseTo(token);
        }
    }
}
