using System;

namespace Common.Runtime.Serialization.IntegTests
{
    using Attributes;

    public class TestNullableClass
    {
        [TestProperty(TestClass.STRING_ATTR_NAME)]
        public string StringParam { get; set; } = TestClass.STRING_VALUE;

        [TestProperty(TestClass.BOOL_ATTR_NAME)]
        public bool? BoolParam { get; set; } = TestClass.BOOL_VALUE;

        [TestProperty(TestClass.BYTE_ATTR_NAME)]
        public byte? ByteParam { get; set; } = TestClass.BYTE_VALUE;

        [TestProperty(TestClass.SHORT_ATTR_NAME)]
        public short? ShortParam { get; set; } = TestClass.SHORT_VALUE;

        [TestProperty(TestClass.INT_ATTR_NAME)]
        public int? IntParam { get; set; } = TestClass.INT_VALUE;

        [TestProperty(TestClass.LONG_ATTR_NAME)]
        public long? LongParam { get; set; } = TestClass.LONG_VALUE;

        [TestProperty(TestClass.FLOAT_ATTR_NAME)]
        public float? FloatParam { get; set; } = TestClass.FLOAT_VALUE;

        [TestProperty(TestClass.DOUBLE_ATTR_NAME)]
        public double? DoubleParam { get; set; } = TestClass.DOUBLE_VALUE;

    }
}
