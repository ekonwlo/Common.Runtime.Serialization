using System;

namespace Common.Runtime.Serialization.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class SerializablePropertyAttribute
        : Attribute, ISerializableProperty
    {
	    public string Name { get; private set; }
		public bool Mandatory { get; set; }
	    public string Version { get; set; }

	    protected internal SerializablePropertyAttribute(string name) 
        {
            Name = name;
            Mandatory = true;
        }
    }
}
