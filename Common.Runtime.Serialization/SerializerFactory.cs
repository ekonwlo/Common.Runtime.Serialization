using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.Reflection;

namespace Common.Runtime.Serialization
{
    using Attributes;
    using Transformation;
    using Parsers;

    public abstract class SerializerFactory<T>
        : ISerializerFactory
    {
        public delegate ISerializer[] CreateSerializersDelegate<U>(TypeDefinition type) where U : ISerializableProperty;

        private readonly SerializersMap<T> _hash;

        internal protected ParserRepository<T> Parsers { get; private set; }

        protected SerializerFactory()
        {
            _hash = new SerializersMap<T>();
            Parsers = new ParserRepository<T>();
        }

        public abstract ISerializer[] Create<U>(Type type) where U : ISerializableProperty;

        internal protected ISerializer<T>[] CreateSerializers<U>(TypeDefinition type) where U : ISerializableProperty
        {
            return (from PropertyInfo prop in type.GetProperties()
                    let converter = CreateConverter<U>(prop)
                    where converter != null
                    select (Serializer<T>)converter).ToArray();
        }

        internal protected ISerializer<T>[] CreateSerializsers<U>(TypeDefinition type, string version, bool @explicit = false) where U : ISerializableProperty
        {
            if (version == null)
            {
                return CreateSerializers<U>(type);
            }
            else
            {
                return (from PropertyInfo prop in type.GetProperties()
                        let converter = CreateConverter<U>(prop, version, @explicit)
                        where converter != null
                        select (Serializer<T>)converter).ToArray();
            }
        }

        protected abstract ISerializer<T> CreatePrimitiveConverter(TypeDefinition type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator);
        protected abstract ISerializer<T> CreateNullableConverter(TypeDefinition type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator);
        protected abstract ISerializer<T> CreateArrayConverter(TypeDefinition type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ISerializer<T> serializer);
        protected abstract ISerializer<T> CreateObjectConverter(TypeDefinition type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ConstructorInfo constructor, CreateSerializersDelegate<ISerializableProperty> createSerializersCallback);
        protected abstract ISerializer<T> CreateDynamicConverter(TypeDefinition type, PropertyInfo property, ISerializableProperty attribute, string format, TransofmationSelector selector, IDictionary<Type, ISerializer<T>> serializers);

        internal virtual ISerializer<T> CreateConverter<U>(TypeDefinition type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator) where U : ISerializableProperty
        {
            if (type.IsClass && !(((Type)type) == typeof(string) || type.IsArray))
            {
                Serializer<T> serializer = _hash[type, property, attribute];
                if (serializer != null)
                    return serializer;
            }

            if (transformator != null)
                type = transformator.OutputType;

            Type baseType = Nullable.GetUnderlyingType(type);

            if (type.IsPrimitive || ((Type)type) == typeof(string))
            {
                return CreatePrimitiveConverter(type, property, attribute, format, transformator);
            }
            else if (baseType != null)
            {
                return CreateNullableConverter(type, property, attribute, format, transformator);
            }
            else if (type.IsArray)
            {
                ISerializer<T> serializer = CreateConverter<U>(Utils.GetArrayBaseType(type), property, attribute, format, null);

                return CreateArrayConverter(type, property, attribute, format, transformator, serializer);
            }
            else if (type.IsClass)
            {
                ConstructorInfo constructor = type.GetDefaultConstructor();
                IEnumerable<ISerializer<T>> serializers = CreateSerializers<U>(type);
                return CreateObjectConverter(type, property, attribute, format, transformator, constructor, CreateSerializers<U>);
            }

            return null;
        }

        private ISerializer<T> CreateConverter<U>(PropertyInfo prop) where U : ISerializableProperty
        {
            foreach (U attr in prop.GetCustomAttributes(typeof(U), false) as U[])
            {
                Type propertyType = prop.PropertyType;

                Transformator[] transformators = GetTransformators(prop);
                TransofmationSelector selector = GetTransformatorSelector(prop);

                if (transformators.Length > 1 || (selector != null))
                {
                    Dictionary<Type, ISerializer<T>> serializers = new Dictionary<Type, ISerializer<T>>();

                    foreach (Transformator transformator in transformators)
                    {
                        serializers.Add(transformator.OutputType, CreateConverter<U>(propertyType, prop, attr, null, transformator));
                    }

                    if (selector != null)
                    {
                        foreach (Type t in selector.Types)
                        {
                            serializers.Add(t, CreateConverter<U>(t, prop, attr, null, null));
                        }
                    }

                    return CreateDynamicConverter(propertyType, prop, attr, null, selector, serializers);
                }
                else if (transformators.Length > 0) // 
                {
                    return CreateConverter<U>(propertyType, prop, attr, null, transformators[0]);
                }
                else
                {
                    return CreateConverter<U>(propertyType, prop, attr, null, null);
                }
            }
            return null;
        }

        private ISerializer<T> CreateConverter<U>(PropertyInfo prop, string version, bool @explicit = false) where U : ISerializableProperty
        {

            foreach (U attr in prop.GetCustomAttributes(typeof(U), false) as U[])
            {
                if (version.Equals(attr.Version) || (attr.Version == null && !@explicit))
                {

                    Type propertyType = prop.PropertyType;

                    Transformator[] transformators = GetTransformators(prop);
                    TransofmationSelector selector = GetTransformatorSelector(prop);

                    if (transformators.Length > 1 || (selector != null))
                    {
                        Dictionary<Type, ISerializer<T>> serializers = new Dictionary<Type, ISerializer<T>>();

                        foreach (Transformator transformator in transformators)
                        {
                            serializers.Add(transformator.OutputType, CreateConverter<U>(propertyType, prop, attr, null, transformator));
                        }

                        if (selector != null)
                        {
                            foreach (Type t in selector.Types)
                            {
                                serializers.Add(t, CreateConverter<U>(t, prop, attr, null, null));
                            }
                        }

                        return CreateDynamicConverter(propertyType, prop, attr, null, selector, serializers);
                    }
                    else if (transformators.Length > 0) // 
                    {
                        return CreateConverter<U>(propertyType, prop, attr, null, transformators[0]);
                    }
                    else
                    {
                        return CreateConverter<U>(propertyType, prop, attr, null, null);
                    }
                }
            }

            return null;
        }

        private TransofmationSelector GetTransformatorSelector(TypeDefinition type, PropertyInfo property, PropertyTransformationAttribute attribute)
        {
            if (attribute.DynamicExpression != null)
            {
                PropertyInfo expression = type.GetProperty(attribute.DynamicExpression);

                return new TransofmationSelector((Delegate)expression.GetValue(attribute.Type.GetConstructor(Type.EmptyTypes).Invoke(null), null), attribute.DynamicTypes);
            }
            return null;
        }

        private TransofmationSelector GetTransformatorSelector(PropertyInfo property)
        {
            foreach (PropertyTransformationAttribute attr in property.GetCustomAttributes(typeof(PropertyTransformationAttribute), false))
            {
                return GetTransformatorSelector(attr.Type, property, attr);
            }
            return null;
        }

        private Transformator GetTransformator(Type type, PropertyInfo property, PropertyTransformationAttribute attribute)
        {
            //if (type == null)
            //    throw new RestException("");

            Type[] genericArguments = type.GetGenericArguments();

            if (genericArguments[0] != property.PropertyType)
                throw new ArgumentException("Transformation must match property argument.");

            return new Transformator(attribute.Type
                                    , type.GetMethod("Transform")
                                    , type.GetMethod("Revert"));
        }

        private Transformator[] GetTransformators(PropertyInfo property)
        {
            foreach (PropertyTransformationAttribute attr in property.GetCustomAttributes(typeof(PropertyTransformationAttribute), false))
            {
                //Type[] interfaces = attr.Type.GetInterfaces();

                return (from type in attr.Type.GetInterfaces()
                        where TypeDefinition.Of(type).IsGenericType && type.Name == "IRestTransformation`2" && type.GetGenericArguments()[0] == property.PropertyType
                        select GetTransformator(type, property, attr)).ToArray();
            }
            return new Transformator[] { };
        }

        internal void Add(Serializer<T> serializer)
        {
            _hash.Add(serializer);
        }
    }
}
