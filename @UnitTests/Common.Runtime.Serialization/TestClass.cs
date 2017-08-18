namespace Common.Runtime.Serialization.UnitTests
{
    public class TestClass
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

        // VALUES
        public static readonly string STRING_VALUE = "string_value";
        public static readonly bool BOOL_VALUE = false;
        public static readonly byte BYTE_VALUE = 32;
        public static readonly short SHORT_VALUE = 16345;
        public static readonly int INT_VALUE = 15;
        public static readonly long LONG_VALUE = 150000;
        public static readonly float FLOAT_VALUE = 1544545f;
        public static readonly double DOUBLE_VALUE = 23534454567567d;

        public string StringParam { get; set; } = STRING_VALUE;
        public bool BoolParam { get; set; } = BOOL_VALUE;
        public byte ByteParam { get; set; } = BYTE_VALUE;
        public short ShortParam { get; set; } = SHORT_VALUE;
        public int IntParam { get; set; } = INT_VALUE;
        public long LongParam { get; set; } = LONG_VALUE;
        public float FloatParam { get; set; } = FLOAT_VALUE;
        public double DoubleParam { get; set; } = DOUBLE_VALUE;

        public string[][] ArrayStringParam { get; set; }
        
    }
}
