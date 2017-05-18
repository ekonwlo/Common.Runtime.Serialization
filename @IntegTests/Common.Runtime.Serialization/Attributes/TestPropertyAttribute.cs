namespace Common.Runtime.Serialization.IntegTests.Attributes
{
    using Serialization.Attributes;

    class TestPropertyAttribute
        : SerializablePropertyAttribute
    {
        public TestPropertyAttribute(string name)
            : base(name)
        { }
    }
}
