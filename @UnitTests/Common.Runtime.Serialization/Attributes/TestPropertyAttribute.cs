namespace Common.Runtime.Serialization.UnitTests.Attributes
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
