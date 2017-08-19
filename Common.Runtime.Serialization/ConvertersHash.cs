using System;
using System.Reflection;
using System.Collections.Generic;

namespace Common.Runtime.Serialization
{
    using Attributes;

    internal sealed class ConvertersHash<T>
    {
        private readonly IDictionary<Key, Serializer<T>> _serializers;

        internal Serializer<T> this[Type type
                                     , PropertyInfo property
                                     , ISerializableProperty attribute
                                     //, string format
                                     //, Transformator transformator
                                     ]
        {
            get
            {
                Key key = new Key(type, property, attribute);
                if (_serializers.ContainsKey(key))
                {
                    return _serializers[key];
                }
                return null;
            }
        }

        internal ConvertersHash()
        {
            _serializers = new Dictionary<Key, Serializer<T>>();
        }
        
        internal void Add(Serializer<T> serializer)
        {
            Key key = new Key(serializer);

            if (!_serializers.ContainsKey(key))
            {
                _serializers.Add(key, serializer);
            }
        }

        private class Key
        {
            Guid TypeGuid { get; set; }
            Guid PropertyTypeGuid { get; set; }
            String PropertyName { get; set; }
            String AttributeName { get; set; }

            public Key(Type type, PropertyInfo property, ISerializableProperty attribute)
            {
                TypeGuid = type.GUID;
                PropertyTypeGuid = property.DeclaringType.GUID;
                PropertyName = property.Name;
                AttributeName = attribute.Name;
            }

            public Key(Serializer<T> serializer)
                : this(serializer.Type, serializer.Property, serializer.Attribute)
            { }

            public override bool Equals(object obj)
            {
                if (obj == null) return false;
                Key other = obj as Key;
                if (other == null) return false;

                if (TypeGuid == other.TypeGuid
                    && PropertyTypeGuid == other.PropertyTypeGuid
                    && PropertyName == other.PropertyName
                    && AttributeName == other.AttributeName) return true;

                return false;
            }

            public override int GetHashCode()
            {
                int hash = 17;
                hash = hash * 23 + TypeGuid.GetHashCode();
                hash = hash * 23 + PropertyTypeGuid.GetHashCode();
                hash = hash * 23 + PropertyName.GetHashCode();
                hash = hash * 23 + AttributeName.GetHashCode();

                return hash;
            }
        }
    }
}
