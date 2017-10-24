using System;
using System.Collections.Generic;
using System.Reflection;
using Common.Reflection;

namespace Common.Runtime.Serialization
{
    using Attributes;
    using Transformation;

    public abstract class ArraySerializer<T>
        : Serializer<T>
    {
        private static readonly TypeDefinition ARRAY_SERIALIZER_TYPE = TypeDefinition.Of<ArraySerializer<T>>();

        private readonly int _dimiensions;
        private readonly Dictionary<int, MethodInfo> _getElementsMethods;
        private readonly Dictionary<int, MethodInfo> _setElementsMethods;

        public ISerializer<T> ElementSerializer { get; private set; }

        public int Dimiensions
        {
            get { return _dimiensions; }
        }

        public TypeDefinition ElementType { get; private set; }

        protected ArraySerializer(SerializerFactory<T> factory, TypeDefinition type, PropertyInfo property, ISerializableProperty attribute, string format, Transformator transformator, ISerializer<T> elementSerializer)
            : base(factory, type, property, attribute, format, transformator)
        {
            if (elementSerializer == null) throw new ArgumentNullException("elementSerializer", "Element Serializer is required");

            _dimiensions = 1;
            ElementType = ((Type) type).GetElementType();

            MethodInfo getElementsMethod = ARRAY_SERIALIZER_TYPE.GetMethod("GetElements", TypeDefinition.Of<T>(), TypeDefinition.IntType);
            _getElementsMethods = new Dictionary<int, MethodInfo>();

            MethodInfo setElementsMethod = ARRAY_SERIALIZER_TYPE.GetMethod("SetElements", TypeDefinition.GenericArrayType, TypeDefinition.IntType);
            
            _setElementsMethods = new Dictionary<int, MethodInfo>();
            _setElementsMethods.Add(_dimiensions, setElementsMethod.MakeGenericMethod(ElementType));

            while (ElementType.IsArray)
            {
                _getElementsMethods.Add(_dimiensions, getElementsMethod.MakeGenericMethod(ElementType));
                ElementType = ((Type)ElementType).GetElementType();
                _dimiensions++;
                _setElementsMethods.Add(_dimiensions, setElementsMethod.MakeGenericMethod(ElementType));

            }
            _getElementsMethods.Add(_dimiensions, getElementsMethod.MakeGenericMethod(ElementType));

            ElementSerializer = elementSerializer;
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