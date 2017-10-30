namespace Common.Runtime.Serialization.Parsers
{
    internal abstract class StringParser<T> : IParser<T, string>
    {
        public abstract T ParseFrom(string input);

        public abstract string ParseTo(T input);
    }
}
