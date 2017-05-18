namespace Common.Runtime.Serialization
{
    using Attributes;

    public abstract class BaseConverter
    {
        public abstract string Name { get; }
       
		public abstract string ToString(object item);
        public abstract object FromString(string text);
        //public abstract ISerializableProperty Attribute { get; private set; }

        public abstract string GetPropertyString(object item);
        public abstract void SetPropertyString(object item, string value);
    }
}
