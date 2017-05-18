namespace Common.Runtime.Serialization.IntegTests
{
    using Attributes;

    class TestClass
    {
        // ATTRIBUTES NAMES
        public const string OBJECT_ATTR_NAME = "object_param";
        public const string STRING_ATTR_NAME = "string_param";
        public const string BOOL_ATTR_NAME = "bool_param";
        public const string BYTE_ATTR_NAME = "byte_param";
        public const string SHORT_ATTR_NAME = "short_param";
        public const string INT_ATTR_NAME = "int_param";
        public const string LONG_ATTR_NAME = "long_param";
        public const string FLOAT_ATTR_NAME = "float_param";
        public const string DOUBLE_ATTR_NAME = "double_param";

        public const string STRING_ARRAY_ATTR_NAME = "string_array_param";

        // VALUES
        public static readonly string STRING_VALUE = "value";
        public static readonly bool BOOL_VALUE = true;
        public static readonly byte BYTE_VALUE = 64;
        public static readonly short SHORT_VALUE = 16000;
        public static readonly int INT_VALUE = 15;
        public static readonly long LONG_VALUE = 32500000;
        public static readonly float FLOAT_VALUE = 164564f;
        public static readonly double DOUBLE_VALUE = 16675567567557d;

        public static readonly string OTHER_STRING_VALUE = "other_value";
        public static readonly bool OTHER_BOOL_VALUE = false;
        public static readonly byte OTHER_BYTE_VALUE = 128;
        public static readonly short OTHER_SHORT_VALUE = -16000;
        public static readonly int OTHER_INT_VALUE = -15;
        public static readonly long OTHER_LONG_VALUE = -32500000;
        public static readonly float OTHER_FLOAT_VALUE = -164564f;
        public static readonly double OTHER_DOUBLE_VALUE = -16675567567557d;

        [TestProperty(OBJECT_ATTR_NAME)]
        public TestChildClass ObjectParam { get; set; } = new TestChildClass();

        [TestProperty(STRING_ARRAY_ATTR_NAME)]
        public string[][] StringArray { get; set; } = new string[][] { new string[] {"r1c1", "r1c2", "r1c3"}, new string[] { "r2c1", "r2c2", "r2c3" } };

    }
}
