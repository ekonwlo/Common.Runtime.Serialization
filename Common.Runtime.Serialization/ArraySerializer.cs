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
        private readonly int _dimiensions;
        private readonly Type _baseType;
        private readonly Dictionary<int, MethodInfo> _getElementsMethods;
        private readonly Dictionary<int, MethodInfo> _setElementsMethods;
        
        public ISerializer<T> BaseSerializer { get; private set; }
        
        public int Dimiensions
        {
            get { return _dimiensions; }
        }

        public Type BaseType
        {
            get { return _baseType;}
        }
        
        protected ArraySerializer(SerializerFactory<T> factory, Type type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ISerializer<T> serializer)
            : base(factory, type, property, attribute, format, transformator) 
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
	        BaseSerializer = serializer;
			//_serializer.Transformator = transformator;
        }

        public MethodInfo GetElementsMethod(int index)
        {
            return _getElementsMethods[index];
        }
       
        public MethodInfo SetElementsMethod(int index)
        {
            return _setElementsMethods[index];
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
        
        protected internal virtual U GetElement<U>(T item)
        {
            throw new NotImplementedException();
        }        
    }
}
