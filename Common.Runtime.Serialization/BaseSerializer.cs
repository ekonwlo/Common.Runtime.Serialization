using System;

namespace Common.Runtime.Serialization
{
    using Attributes;
    using Transformation;

    public abstract class BaseSerializer : ISerializer
    {
        public string Name { get; private set; }
        public ISerializableProperty Attribute { get; private set; }
        public Transformator Transformator { get; private set; }

        internal protected BaseSerializer(ISerializableProperty attribute, Transformator transformator)
        {
            if (attribute == null) throw new ArgumentNullException("attribute", "Attribute is required");
            if (attribute.Name == null) throw new ArgumentNullException("attribute", "Attribute Name is required");

            Attribute = attribute;
            Name = Attribute.Name;
            Transformator = transformator;
        }
        public abstract object GetPropertyValue(object item);
        public abstract void SetPropertyValue(object item, object value);

        public abstract U GetProperty<U>(object item);
        public abstract void SetProperty<U>(object item, U value);

        public abstract T To<T>(object item);
        public abstract object From<T>(T value);
    }
}
