namespace Common.Runtime.Serialization
{
    using Attributes;
    using Transformation;

    public interface ISerializer
    {
        ISerializableProperty Attribute { get; }

        string Name { get; }

        Transformator Transformator { get; }

        object GetPropertyValue(object item);

        void SetPropertyValue(object item, object value);
    }

    public interface ISerializer<T> : ISerializer
    {
        ISerializer<T> this[object item] { get; }

        object GetElementValue(T item);

        T SetElementValue(object item);

        object ConvertToObject(T item);

        T ConvertFromObject(object item);
    }
}
