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

        U GetProperty<U>(object item);

        void SetProperty<U>(object item, U value);

        T To<T>(object item);

        object From<T>(T value);
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
