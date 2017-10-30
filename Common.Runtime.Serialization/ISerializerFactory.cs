using System;

namespace Common.Runtime.Serialization
{
    using Attributes;

    public interface ISerializerFactory
    {
        ISerializer[] Create<U>(Type type) where U : ISerializableProperty;
    }

    [Obsolete()]
    public interface ISerializerFactory<T> : ISerializerFactory
    { }
}
