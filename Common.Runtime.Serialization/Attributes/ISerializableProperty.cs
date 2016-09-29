using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Runtime.Serialization.Attributes
{
    public interface ISerializableProperty
    {
        string Name { get; }
        bool Mandatory { get; }
	    string Version { get; }
    }
}
