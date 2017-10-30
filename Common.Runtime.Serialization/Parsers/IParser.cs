namespace Common.Runtime.Serialization.Parsers
{
    public interface IParser
    { }

    public interface IParser<T, U> : IParser
    {
        T ParseFrom(U input);

        U ParseTo(T input);
    }
}
