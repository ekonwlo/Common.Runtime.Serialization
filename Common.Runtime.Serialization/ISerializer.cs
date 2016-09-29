namespace Common.Runtime.Serialization
{
	using Transformation;

	public interface ISerializer<T>
	{

		ISerializer<T> this[object item] { get; }
		string Name { get; }
		Transformator Transformator { get; }

		object GetPropertyValue(object item);
		void SetPropertyValue(object item, object value);

		object GetElementValue(T item);
		T SetElementValue(object item);

		object ConvertToObject(T item);
		T ConvertFromObject(object item);
	}
}
