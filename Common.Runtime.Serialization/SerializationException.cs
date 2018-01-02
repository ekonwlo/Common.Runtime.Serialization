using System;

namespace Common.Runtime.Serialization
{
    [Serializable]
    public class SerializationException : Exception
    {
        internal SerializationException(string message)
            : base(message)
        { }

        internal SerializationException(string message, Exception cause)
            : base(message, cause)
        { }

    }
}
