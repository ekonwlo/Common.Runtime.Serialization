using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Runtime.Serialization
{
    using Attributes;
    using Transformation;

    public abstract class SerializerFactory<T>
		: ISerializerFactory<T>
    {

        private static object _sync = new object();

		private delegate ISerializer<T>[] CreateDelegate<U>(Type type) where U : ISerializableProperty;
		private delegate ISerializer<T> CreateConverterDelegate<U>(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator) where U : ISerializableProperty;

        private static CreateDelegate<ISerializableProperty> CreateCallback { get; set; }
        private static CreateConverterDelegate<ISerializableProperty> CreateConverterCallback { get; set; }

        private static readonly  ConvertersHash<T> _hash;

        static SerializerFactory()
        {
            _hash = new ConvertersHash<T>();

        }

		internal static ISerializer<T>[] Create(Type type)
        {
            return CreateCallback(type);
        }

		public abstract ISerializer<T>[] Create<U>(Type type) where U : ISerializableProperty;

		protected ISerializer<T>[] CreateConverters<U>(Type type) where U : ISerializableProperty
        {
            lock (_sync) // prevents from changing callback while creating converters
            {
                SetDefault<U>(this);

                return (from PropertyInfo prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty)
                        let converter = CreateConverter<U>(prop)
                        where converter != null
                        select (Serializer<T>)converter).ToArray();
            }
        }

		protected ISerializer<T>[] CreateConverters<U>(Type type, string version, bool @explicit = false) where U : ISerializableProperty
		{
			if (version == null)
			{
				return CreateConverters<U>(type);
			}
			else
			{
				lock (_sync) // prevents from changing callback while creating converters
				{
					SetDefault<U>(this);

					return
					(from PropertyInfo prop in
						type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty |
						                   BindingFlags.SetProperty)
						let converter = CreateConverter<U>(prop, version, @explicit)
						where converter != null
						select (Serializer<T>) converter).ToArray();
				}
			}
		}

		protected abstract ISerializer<T> CreatePrimitiveConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator); //where U : RestAttribute;
		protected abstract ISerializer<T> CreateNullableConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator); //where U : RestAttribute;
		public abstract ISerializer<T> CreateArrayConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ISerializer<T> serializer); //where U : RestAttribute;
		public abstract ISerializer<T> CreateObjectConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ConstructorInfo constructor, IEnumerable<ISerializer<T>> serializers); //where U : RestAttribute;
		protected abstract ISerializer<T> CreateDynamicConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, TransofmationSelector selector, IDictionary<Type, ISerializer<T>> serializers); //where U : RestAttribute;

		internal static ISerializer<T> CreateConverter(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator)
        {
            return CreateConverterCallback(type, property, attribute, format, transformator);
        }

		internal virtual ISerializer<T> CreateConverter<U>(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator) where U : ISerializableProperty
        {
            if (type.IsClass && !( type == typeof(string) || type.IsArray ))
            {
                Serializer<T> serializer = _hash[type, property, attribute];
                if (serializer != null)
                    return serializer;
            }
            
            if (transformator != null)
                type = transformator.OutputType;

            Type baseType = Nullable.GetUnderlyingType(type);

            if (type.IsPrimitive || type == typeof(string))
            {
                return CreatePrimitiveConverter(type, property, attribute, format, transformator);
            }
            else if (baseType != null)
            {
                return CreateNullableConverter(type, property, attribute, format, transformator);
            }
            else if (type.IsArray)
            {
				ISerializer<T> serializer = CreateConverter(Utils.GetBaseArrayType(type), property, attribute, format, null);
				//serializer.Transformator = transformator;
            
                return CreateArrayConverter(type, property, attribute, format, transformator, serializer);
            }
            else if (type.IsClass)
            {
                
                ConstructorInfo constructor = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
				IEnumerable<ISerializer<T>> serializers = Create(type);
				return CreateObjectConverter(type, property, attribute, format, transformator, constructor, serializers);
            }

            return null;
        }

		private ISerializer<T> CreateConverter<U>(PropertyInfo prop) where U : ISerializableProperty
        {
            foreach (U attr in prop.GetCustomAttributes(typeof(U), false))
            {
                Type propertyType = prop.PropertyType;

                Transformator[] transformators = GetTransformators(prop);
                TransofmationSelector selector = GetTransformatorSelector(prop);
                
                if (transformators.Length > 1 || (selector != null) )
                {
					Dictionary<Type, ISerializer<T>> serializers = new Dictionary<Type, ISerializer<T>>();

					foreach (Transformator transformator in transformators)
					{
						serializers.Add(transformator.OutputType, CreateConverter(propertyType, prop, attr, null, transformator));
					}

					if (selector != null)
					{
						foreach (Type t in selector.Types)
						{
							serializers.Add(t, CreateConverter(t, prop, attr, null, null));
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
			
			foreach (U attr in prop.GetCustomAttributes(typeof(U), false))
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
							serializers.Add(transformator.OutputType, CreateConverter(propertyType, prop, attr, null, transformator));
						}

						if (selector != null)
						{
							foreach (Type t in selector.Types)
							{
								serializers.Add(t, CreateConverter(t, prop, attr, null, null));
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

        private TransofmationSelector GetTransformatorSelector(Type type, PropertyInfo property, PropertyTransformationAttribute attribute)
        {
            //if (type == null)
            //    throw new RestException("");
            if (attribute.DynamicExpression != null)
            {
                PropertyInfo expression = type.GetProperty(attribute.DynamicExpression, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

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

                return (from Type type in attr.Type.GetInterfaces()
                        where type.IsGenericType && type.Name == "IRestTransformation`2" && type.GetGenericArguments()[0] == property.PropertyType
                        select GetTransformator(type, property, attr)).ToArray();
            }
            return new Transformator[] { };
        }

        public static void SetDefault<U>(SerializerFactory<T> factory) where U : ISerializableProperty
        {
            lock (_sync) // prevents from changing callback while creating converters
            {
                CreateCallback = factory.Create<U>;
                CreateConverterCallback = factory.CreateConverter<U>;
            }
        }

        internal static void AddConverter(Serializer<T> serializer)
        {
            _hash.Add(serializer);
        }
    }
}
