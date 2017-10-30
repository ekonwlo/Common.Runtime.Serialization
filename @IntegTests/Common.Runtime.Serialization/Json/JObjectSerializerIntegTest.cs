using Xunit;
using Newtonsoft.Json.Linq;

namespace Common.Runtime.Serialization.IntegTests.Json
{
    using Serialization.Json;
    using Attributes;

    public class JObjectSerializerIntegTest
    {
        private static readonly ISerializerFactory FACTORY = new JSerializerFactory();
        private static readonly ISerializer<JToken> INSTANCE = (ISerializer<JToken>) FACTORY.Create<TestPropertyAttribute>(typeof(TestClass))[0];

        private static readonly JToken JOBJECT = new JObject(
                new JProperty(TestClass.STRING_ATTR_NAME, new JValue(TestClass.OTHER_STRING_VALUE)),
                new JProperty(TestClass.BOOL_ATTR_NAME, new JValue(TestClass.OTHER_BOOL_VALUE)),
                new JProperty(TestClass.BYTE_ATTR_NAME, new JValue(TestClass.OTHER_BYTE_VALUE)),
                new JProperty(TestClass.SHORT_ATTR_NAME, new JValue(TestClass.OTHER_SHORT_VALUE)),
                new JProperty(TestClass.INT_ATTR_NAME, new JValue(TestClass.OTHER_INT_VALUE)),
                new JProperty(TestClass.LONG_ATTR_NAME, new JValue(TestClass.OTHER_LONG_VALUE)),
                new JProperty(TestClass.FLOAT_ATTR_NAME, new JValue(TestClass.OTHER_FLOAT_VALUE)),
                new JProperty(TestClass.DOUBLE_ATTR_NAME, new JValue(TestClass.OTHER_DOUBLE_VALUE))
        );
        private static readonly JToken JPROPERTY = new JProperty(TestClass.OBJECT_ATTR_NAME, JOBJECT);

        [Fact(DisplayName = "Should convert from object to jobject")]
        public void ShouldConvertFromObjectToXElement()
        {
            var o = new TestClass();

            var converted = INSTANCE.ConvertFromObject(o);

            Assert.IsAssignableFrom<JToken>(converted);
            Assert.IsAssignableFrom<JProperty>(converted);

            var element = converted as JProperty;
            Assert.Equal(TestClass.OBJECT_ATTR_NAME, element.Name);

            var stringElement = element.Value[TestClass.STRING_ATTR_NAME] as JValue;
            var boolElement = element.Value[TestClass.BOOL_ATTR_NAME] as JValue;
            var byteElement = element.Value[TestClass.BYTE_ATTR_NAME] as JValue;
            var shortElement = element.Value[TestClass.SHORT_ATTR_NAME] as JValue;
            var intElement = element.Value[TestClass.INT_ATTR_NAME] as JValue;
            var longElement = element.Value[TestClass.LONG_ATTR_NAME] as JValue;
            var floatElement = element.Value[TestClass.FLOAT_ATTR_NAME] as JValue;
            var doubleElement = element.Value[TestClass.DOUBLE_ATTR_NAME] as JValue;


            Assert.NotNull(stringElement);
            Assert.NotNull(boolElement);
            Assert.NotNull(byteElement);
            Assert.NotNull(shortElement);
            Assert.NotNull(intElement);
            Assert.NotNull(longElement);
            Assert.NotNull(floatElement);
            Assert.NotNull(doubleElement);


            Assert.Equal(TestClass.STRING_VALUE, stringElement.Value);
            Assert.Equal(TestClass.BOOL_VALUE, boolElement.Value);
            Assert.Equal(TestClass.BYTE_VALUE, byteElement.Value);
            Assert.Equal(TestClass.SHORT_VALUE, shortElement.Value);
            Assert.Equal(TestClass.INT_VALUE, intElement.Value);
            Assert.Equal(TestClass.LONG_VALUE, longElement.Value);
            Assert.Equal(TestClass.FLOAT_VALUE, floatElement.Value);
            Assert.Equal(TestClass.DOUBLE_VALUE, doubleElement.Value);
        }

        [Fact(DisplayName = "Should convert from jobject to object")]
        public void ShouldConvertFromJObjectToObject()
        {
            var converted = INSTANCE.ConvertToObject(JOBJECT);

            Assert.NotNull(converted);
            Assert.IsAssignableFrom<TestChildClass>(converted);

            var child = converted as TestChildClass;
            Assert.Equal(TestClass.OTHER_STRING_VALUE, child.StringParam);
            Assert.Equal(TestClass.OTHER_BOOL_VALUE, child.BoolParam);
            Assert.Equal(TestClass.OTHER_BYTE_VALUE, child.ByteParam);
            Assert.Equal(TestClass.OTHER_SHORT_VALUE, child.ShortParam);
            Assert.Equal(TestClass.OTHER_INT_VALUE, child.IntParam);
            Assert.Equal(TestClass.OTHER_LONG_VALUE, child.LongParam);
            Assert.Equal(TestClass.OTHER_FLOAT_VALUE, child.FloatParam);
            Assert.Equal(TestClass.OTHER_DOUBLE_VALUE, child.DoubleParam);
        }
    }
}
