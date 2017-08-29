using System;
using System.Reflection;
using Xunit;
using NSubstitute;
using Newtonsoft.Json.Linq;
using Common.Reflection;

namespace Common.Runtime.Serialization.UnitTests.Json
{
    using Serialization.Attributes;
    using Serialization.Json;

    public class JPrimitiveSerializerTest
    {
        private static readonly SerializerFactory<JToken> FACTORY = Substitute.For<SerializerFactory<JToken>>();

        private static readonly Type TEST_TYPE = typeof(TestClass);

        private static readonly PropertyInfo STRING_PROP = TEST_TYPE.GetProperty("StringParam");
        private static readonly ISerializableProperty STRING_ATTR = Substitute.For<ISerializableProperty>();

        private static readonly PropertyInfo BOOL_PROP = TEST_TYPE.GetProperty("BoolParam");
        private static readonly ISerializableProperty BOOL_ATTR = Substitute.For<ISerializableProperty>();

        private static readonly PropertyInfo BYTE_PROP = TEST_TYPE.GetProperty("ByteParam");
        private static readonly ISerializableProperty BYTE_ATTR = Substitute.For<ISerializableProperty>();

        private static readonly PropertyInfo SHORT_PROP = TEST_TYPE.GetProperty("ShortParam");
        private static readonly ISerializableProperty SHORT_ATTR = Substitute.For<ISerializableProperty>();

        private static readonly PropertyInfo INT_PROP = TEST_TYPE.GetProperty("IntParam");
        private static readonly ISerializableProperty INT_ATTR = Substitute.For<ISerializableProperty>();

        private static readonly PropertyInfo LONG_PROP = TEST_TYPE.GetProperty("LongParam");
        private static readonly ISerializableProperty LONG_ATTR = Substitute.For<ISerializableProperty>();

        private static readonly PropertyInfo FLOAT_PROP = TEST_TYPE.GetProperty("FloatParam");
        private static readonly ISerializableProperty FLOAT_ATTR = Substitute.For<ISerializableProperty>();

        private static readonly PropertyInfo DOUBLE_PROP = TEST_TYPE.GetProperty("DoubleParam");
        private static readonly ISerializableProperty DOUBLE_ATTR = Substitute.For<ISerializableProperty>();

        static JPrimitiveSerializerTest()
        {
            STRING_ATTR.Name.Returns(TestClass.STRING_ATTR_NAME);
            BOOL_ATTR.Name.Returns(TestClass.BOOL_ATTR_NAME);
            BYTE_ATTR.Name.Returns(TestClass.BYTE_ATTR_NAME);
            SHORT_ATTR.Name.Returns(TestClass.SHORT_ATTR_NAME);
            INT_ATTR.Name.Returns(TestClass.INT_ATTR_NAME);
            LONG_ATTR.Name.Returns(TestClass.LONG_ATTR_NAME);
            FLOAT_ATTR.Name.Returns(TestClass.FLOAT_ATTR_NAME);
            DOUBLE_ATTR.Name.Returns(TestClass.DOUBLE_ATTR_NAME);
        }

        [Fact(DisplayName = "Should create instance")]
        public void ShouldCreateInstance()
        {
            var instance = new JPrimitiveSerializer(FACTORY, typeof(string), STRING_PROP, STRING_ATTR, null, null);

            Assert.IsAssignableFrom<ISerializer<JToken>>(instance);
            Assert.Equal(TypeDefinition.StringType, instance.Type);
            Assert.Equal(TestClass.STRING_ATTR_NAME, instance.Name);
            Assert.Equal(STRING_PROP, instance.Property);
            Assert.Equal(STRING_ATTR, instance.Attribute);
        }

        [Fact(DisplayName = "Should throw on invalid arguments")]
        public void ShouldThrowOnInvalidArguments()
        {
            Assert.Throws<ArgumentNullException>(() => { new JPrimitiveSerializer(null, typeof(string), STRING_PROP, STRING_ATTR, null, null); });
            Assert.Throws<ArgumentNullException>(() => { new JPrimitiveSerializer(FACTORY, null, STRING_PROP, STRING_ATTR, null, null); });
            Assert.Throws<ArgumentNullException>(() => { new JPrimitiveSerializer(FACTORY, typeof(string), null, STRING_ATTR, null, null); });
            Assert.Throws<ArgumentNullException>(() => { new JPrimitiveSerializer(FACTORY, typeof(string), STRING_PROP, null, null, null); });
            Assert.Throws<ArgumentException>(() => { new JPrimitiveSerializer(FACTORY, TEST_TYPE, STRING_PROP, STRING_ATTR, null, null); });
        }

        [Fact(DisplayName = "Should convert string from object")]
        public void ShouldConvertStringFromObject()
        {
            var o = new TestClass();

            var instance = new JPrimitiveSerializer(FACTORY, typeof(string), STRING_PROP, STRING_ATTR, null, null);
            var result = instance.ConvertFromObject(o) as JProperty;

            Assert.NotNull(result);
            Assert.Equal(TestClass.STRING_ATTR_NAME, result.Name);
            Assert.Equal(new JValue(TestClass.STRING_VALUE), result.Value);
        }

        [Fact(DisplayName = "Should convert bool from object")]
        public void ShouldConvertBoolFromObject()
        {
            var o = new TestClass();

            var instance = new JPrimitiveSerializer(FACTORY, typeof(bool), BOOL_PROP, BOOL_ATTR, null, null);
            var result = instance.ConvertFromObject(o) as JProperty;

            Assert.NotNull(result);
            Assert.Equal(TestClass.BOOL_ATTR_NAME, result.Name);
            Assert.Equal(new JValue(TestClass.BOOL_VALUE), result.Value);
        }

        [Fact(DisplayName = "Should convert byte from object")]
        public void ShouldConvertByteFromObject()
        {
            var o = new TestClass();

            var instance = new JPrimitiveSerializer(FACTORY, typeof(byte), BYTE_PROP, BYTE_ATTR, null, null);
            var result = instance.ConvertFromObject(o) as JProperty;

            Assert.NotNull(result);
            Assert.Equal(TestClass.BYTE_ATTR_NAME, result.Name);
            Assert.Equal(new JValue(TestClass.BYTE_VALUE), result.Value);
        }

        [Fact(DisplayName = "Should convert short from object")]
        public void ShouldConvertShortFromObject()
        {
            var o = new TestClass();

            var instance = new JPrimitiveSerializer(FACTORY, typeof(short), SHORT_PROP, SHORT_ATTR, null, null);
            var result = instance.ConvertFromObject(o) as JProperty;

            Assert.NotNull(result);
            Assert.Equal(TestClass.SHORT_ATTR_NAME, result.Name);
            Assert.Equal(new JValue(TestClass.SHORT_VALUE), result.Value);
        }

        [Fact(DisplayName = "Should convert int from object")]
        public void ShouldConvertIntFromObject()
        {
            var o = new TestClass();

            var instance = new JPrimitiveSerializer(FACTORY, typeof(int), INT_PROP, INT_ATTR, null, null);
            var result = instance.ConvertFromObject(o) as JProperty;

            Assert.NotNull(result);
            Assert.Equal(TestClass.INT_ATTR_NAME, result.Name);
            Assert.Equal(new JValue(TestClass.INT_VALUE), result.Value);
        }

        [Fact(DisplayName = "Should convert long from object")]
        public void ShouldConvertLongFromObject()
        {
            var o = new TestClass();

            var instance = new JPrimitiveSerializer(FACTORY, typeof(long), LONG_PROP, LONG_ATTR, null, null);
            var result = instance.ConvertFromObject(o) as JProperty;

            Assert.NotNull(result);
            Assert.Equal(TestClass.LONG_ATTR_NAME, result.Name);
            Assert.Equal(new JValue(TestClass.LONG_VALUE), result.Value);
        }

        [Fact(DisplayName = "Should convert float from object")]
        public void ShouldConvertFloatFromObject()
        {
            var o = new TestClass();

            var instance = new JPrimitiveSerializer(FACTORY, typeof(float), FLOAT_PROP, FLOAT_ATTR, null, null);
            var result = instance.ConvertFromObject(o) as JProperty;

            Assert.NotNull(result);
            Assert.Equal(TestClass.FLOAT_ATTR_NAME, result.Name);
            Assert.Equal(new JValue(TestClass.FLOAT_VALUE), result.Value);
        }

        [Fact(DisplayName = "Should convert double from object")]
        public void ShouldConvertDoubleFromObject()
        {
            var o = new TestClass();

            var instance = new JPrimitiveSerializer(FACTORY, typeof(double), DOUBLE_PROP, DOUBLE_ATTR, null, null);
            var result = instance.ConvertFromObject(o) as JProperty;

            Assert.NotNull(result);
            Assert.Equal(TestClass.DOUBLE_ATTR_NAME, result.Name);
            Assert.Equal(new JValue(TestClass.DOUBLE_VALUE), result.Value);
        }

        [Fact(DisplayName = "Should convert jproperty to string")]
        public void ShouldConvertJPropertyToString()
        {
            var element = new JProperty(TestClass.STRING_ATTR_NAME, new JValue(TestClass.STRING_VALUE));
            var instance = new JPrimitiveSerializer(FACTORY, typeof(string), STRING_PROP, STRING_ATTR, null, null);

            var result = instance.ConvertToObject(element);
            Assert.Equal(TestClass.STRING_VALUE, result);
        }

        [Fact(DisplayName = "Should convert jproperty to bool")]
        public void ShouldConvertJPropertyToBool()
        {
            var element = new JProperty(TestClass.BOOL_ATTR_NAME, new JValue(TestClass.BOOL_VALUE));
            var instance = new JPrimitiveSerializer(FACTORY, typeof(bool), BOOL_PROP, BOOL_ATTR, null, null);

            var result = instance.ConvertToObject(element);
            Assert.Equal(TestClass.BOOL_VALUE, result);
        }

        [Fact(DisplayName = "Should convert jproperty to byte")]
        public void ShouldConvertJPropertyToByte()
        {
            var element = new JProperty(TestClass.BYTE_ATTR_NAME, new JValue(TestClass.BYTE_VALUE));
            var instance = new JPrimitiveSerializer(FACTORY, typeof(byte), BYTE_PROP, BYTE_ATTR, null, null);

            var result = instance.ConvertToObject(element);
            Assert.Equal(TestClass.BYTE_VALUE, result);
        }

        [Fact(DisplayName = "Should convert jproperty to short")]
        public void ShouldConvertJPropertyToShort()
        {
            var element = new JProperty(TestClass.SHORT_ATTR_NAME, new JValue(TestClass.SHORT_VALUE));
            var instance = new JPrimitiveSerializer(FACTORY, typeof(short), SHORT_PROP, SHORT_ATTR, null, null);

            var result = instance.ConvertToObject(element);
            Assert.Equal(TestClass.SHORT_VALUE, result);
        }

        [Fact(DisplayName = "Should convert jproperty to int")]
        public void ShouldConvertJPropertyToInt()
        {
            var element = new JProperty(TestClass.INT_ATTR_NAME, new JValue(TestClass.INT_VALUE));
            var instance = new JPrimitiveSerializer(FACTORY, typeof(int), INT_PROP, INT_ATTR, null, null);

            var result = instance.ConvertToObject(element);
            Assert.Equal(TestClass.INT_VALUE, result);
        }

        [Fact(DisplayName = "Should convert jproperty to long")]
        public void ShouldConvertJPropertyToLong()
        {
            var element = new JProperty(TestClass.LONG_ATTR_NAME, new JValue(TestClass.LONG_VALUE));
            var instance = new JPrimitiveSerializer(FACTORY, typeof(long), LONG_PROP, LONG_ATTR, null, null);

            var result = instance.ConvertToObject(element);
            Assert.Equal(TestClass.LONG_VALUE, result);
        }

        [Fact(DisplayName = "Should convert jproperty to float")]
        public void ShouldConvertJPropertyToFloat()
        {
            var element = new JProperty(TestClass.FLOAT_ATTR_NAME, new JValue(TestClass.FLOAT_VALUE));
            var instance = new JPrimitiveSerializer(FACTORY, typeof(float), FLOAT_PROP, FLOAT_ATTR, null, null);

            var result = instance.ConvertToObject(element);
            Assert.Equal(TestClass.FLOAT_VALUE, result);
        }

        [Fact(DisplayName = "Should convert jproperty to double")]
        public void ShouldConvertJPropertyToDouble()
        {
            var element = new JProperty(TestClass.DOUBLE_ATTR_NAME, new JValue(TestClass.DOUBLE_VALUE));
            var instance = new JPrimitiveSerializer(FACTORY, typeof(double), DOUBLE_PROP, DOUBLE_ATTR, null, null);

            var result = instance.ConvertToObject(element);
            Assert.Equal(TestClass.DOUBLE_VALUE, result);
        }
    }
}
