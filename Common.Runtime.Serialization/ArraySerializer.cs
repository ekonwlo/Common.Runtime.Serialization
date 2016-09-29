using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Runtime.Serialization
{
    using Attributes;
    using Transformation;

    public abstract class ArraySerializer<T>
        : Serializer<T>
    {
        //private readonly Serializer<T>[] _converters;
        private readonly int _dimiensions;
        private readonly Type _baseType;
        private readonly ISerializer<T> _serializer;
        private readonly Dictionary<int, MethodInfo> _getElementsMethods;
        private readonly Dictionary<int, MethodInfo> _setElementsMethods;
        private readonly MethodInfo _getGenericElementMethod;
        private readonly MethodInfo _setGenericElementMethod;
        private readonly Type _nullableType;

        public ISerializer<T> Converter
        {
            get { return _serializer; }
        }

        //public IEnumerable<Serializer<T>> Converters
        //{
        //    get { return _converters; }
        //}

        public int Dimiensions
        {
            get { return _dimiensions; }
        }

        public Type BaseType
        {
            get { return _baseType;}
        }

        public Type NullableType
        {
            get { return _nullableType; }
        }

        protected ArraySerializer(Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ISerializer<T> serializer)
            : base(type, property, attribute, format, transformator) 
        {
            _dimiensions = 1;
            _baseType = type.GetElementType();


            MethodInfo getElementsMethod = typeof(ArraySerializer<T>).GetMethod("GetElements", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(T), typeof(int) }, null);
            _getElementsMethods = new Dictionary<int, MethodInfo>();
            
            MethodInfo setElementsMethod = (from MethodInfo method in typeof(ArraySerializer<T>).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                                            where method.Name == "SetElements"
                                            select method).First();
            _setElementsMethods = new Dictionary<int, MethodInfo>();
            _setElementsMethods.Add(_dimiensions, setElementsMethod.MakeGenericMethod(_baseType));

            while (_baseType.IsArray)
            {
                _getElementsMethods.Add(_dimiensions, getElementsMethod.MakeGenericMethod(_baseType));
                _baseType = _baseType.GetElementType();
                _dimiensions++;
                _setElementsMethods.Add(_dimiensions, setElementsMethod.MakeGenericMethod(_baseType));
                
            }
            _getElementsMethods.Add(_dimiensions, getElementsMethod.MakeGenericMethod(_baseType));

            //_serializer = (Serializer<T>) SerializerFactory<T>.CreateConverter(_baseType, property, attribute, format, null);
	        _serializer = serializer;
			//_serializer.Transformator = transformator;
            
           _nullableType = Nullable.GetUnderlyingType(BaseType);
           if (BaseType.IsPrimitive | typeof(string) == BaseType)
           {
               MethodInfo getElementMethod = typeof(ArraySerializer<T>).GetMethod("GetPrimitiveElement", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(T) }, null);
               _getGenericElementMethod = getElementMethod.MakeGenericMethod(new Type[] { BaseType });
               MethodInfo setElementMethod = (from method in typeof(ArraySerializer<T>).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                                              where method.Name == "SetPrimitiveElement"
                                              select method).First();
               _setGenericElementMethod = setElementMethod.MakeGenericMethod(BaseType);
           }
           else if (_nullableType != null)
           {
               MethodInfo getElementMethod = typeof(ArraySerializer<T>).GetMethod("GetNullableElement", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(T) }, null);
               _getGenericElementMethod = getElementMethod.MakeGenericMethod(new Type[] { BaseType });
               MethodInfo setElementMethod = (from method in typeof(ArraySerializer<T>).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                                              where method.Name == "SetNullableElement"
                                              select method).First();
               _setGenericElementMethod = setElementMethod.MakeGenericMethod(BaseType);
           }
           else
           {
               MethodInfo getElementMethod = typeof(ArraySerializer<T>).GetMethod("GetElement", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(T) }, null);
               _getGenericElementMethod = getElementMethod.MakeGenericMethod(new Type[] { BaseType });
               MethodInfo setElementMethod = (from method in typeof(ArraySerializer<T>).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                                              where method.Name == "SetElement"
                                              select method).First();
               _setGenericElementMethod = setElementMethod.MakeGenericMethod(BaseType);
           }     

           //Constructor = _baseType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
        }

        public MethodInfo GetElementsMethod(int index)
        {
            return _getElementsMethods[index];
        }
       
        public MethodInfo SetElementsMethod(int index)
        {
            return _setElementsMethods[index];
        }

        public MethodInfo GetElementMethod()
        {
            return _getGenericElementMethod;
        }

        public MethodInfo SetElementMethod()
        {
            return _setGenericElementMethod;
        }

        public override object ConvertToObject(T item)
        {
            throw new NotImplementedException();
        }

        public override T ConvertFromObject(object item)
        {
            throw new NotImplementedException();
        }

        protected virtual U[] GetElements<U>(T item, int dimension)
        { 
            throw new NotImplementedException();
        }

        protected virtual T SetElements<U>(U[] items, int dimension)
        {
            throw new NotImplementedException();
        }

        protected internal virtual T SetElement<U>(U[] items)
        {
            throw new NotImplementedException();
        }

        protected internal virtual T SetPrimitiveElement<U>(U[] items)
        {
            throw new NotImplementedException();
        }

        protected internal virtual T SetNullableElement<U>(U[] items)
        {
            throw new NotImplementedException();
        }

        protected internal virtual U GetElement<U>(T item)
        {
            throw new NotImplementedException();
        }

        protected internal virtual U GetPrimitiveElement<U>(T item)
        {
            throw new NotImplementedException();
        }

        protected internal virtual U GetNullableElement<U>(T item)
        {
            throw new NotImplementedException();
        }

    }
}
