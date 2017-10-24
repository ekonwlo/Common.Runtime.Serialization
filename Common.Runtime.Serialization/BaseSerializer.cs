using System;

namespace Common.Runtime.Serialization
{
    using Attributes;

    public abstract class BaseSerializer
    {
        public string Name { get; private set; }
        public ISerializableProperty Attribute { get; private set; }

		public abstract string ToString(object item);
        public abstract object FromString(string text);
        
        public abstract string GetPropertyString(object item);
        public abstract void SetPropertyString(object item, string value);

        internal protected BaseSerializer(ISerializableProperty attribute)
        {
            if (attribute == null) throw new ArgumentNullException("attribute", "Attribute is required");
            if (attribute.Name == null) throw new ArgumentNullException("attribute", "Attribute Name is required");

            Attribute = attribute;
            Name = Attribute.Name;
        }


    }
}
