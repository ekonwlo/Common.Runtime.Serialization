using System.Xml.Linq;

using Xunit;

namespace Common.Runtime.Serialization.IntegTests.Xml
{
    using Attributes;
    using Serialization.Xml;

    public class XObjectSerializerIntegTest
    {
        private static readonly ISerializerFactory FACTORY = new XSerializerFactory();
        private static readonly ISerializer<XObject> INSTANCE = (ISerializer<XObject>) FACTORY.Create<TestPropertyAttribute>(typeof(TestClass))[0];
        
        private static readonly XElement XELEMENT = new XElement(TestClass.OBJECT_ATTR_NAME, new XElement[] {
                new XElement(TestClass.STRING_ATTR_NAME, new XText(TestClass.OTHER_STRING_VALUE)),
                new XElement(TestClass.BOOL_ATTR_NAME, new XText(TestClass.OTHER_BOOL_VALUE.ToString())),
                new XElement(TestClass.BYTE_ATTR_NAME, new XText(TestClass.OTHER_BYTE_VALUE.ToString())),
                new XElement(TestClass.SHORT_ATTR_NAME, new XText(TestClass.OTHER_SHORT_VALUE.ToString())),
                new XElement(TestClass.INT_ATTR_NAME, new XText(TestClass.OTHER_INT_VALUE.ToString())),
                new XElement(TestClass.LONG_ATTR_NAME, new XText(TestClass.OTHER_LONG_VALUE.ToString())),
                new XElement(TestClass.FLOAT_ATTR_NAME, new XText(TestClass.OTHER_FLOAT_VALUE.ToString())),
                new XElement(TestClass.DOUBLE_ATTR_NAME, new XText(TestClass.OTHER_DOUBLE_VALUE.ToString())),
            });
        
        [Fact(DisplayName = "Should convert from object to xelement")]
        public void ShouldConvertFromObjectToXElement()
        {
            var o = new TestClass();

            var converted = INSTANCE.ConvertFromObject(o);

            Assert.IsAssignableFrom<XObject>(converted);
            Assert.IsAssignableFrom<XElement>(converted);

            var element = converted as XElement;
            Assert.Equal(TestClass.OBJECT_ATTR_NAME, element.Name);

            var stringElement = element.Element(TestClass.STRING_ATTR_NAME);
            var boolElement = element.Element(TestClass.BOOL_ATTR_NAME);
            var byteElement = element.Element(TestClass.BYTE_ATTR_NAME);
            var shortElement = element.Element(TestClass.SHORT_ATTR_NAME);
            var intElement = element.Element(TestClass.INT_ATTR_NAME);
            var longElement = element.Element(TestClass.LONG_ATTR_NAME);
            var floatElement = element.Element(TestClass.FLOAT_ATTR_NAME);
            var doubleElement = element.Element(TestClass.DOUBLE_ATTR_NAME);
            

            Assert.NotNull(stringElement);
            Assert.NotNull(boolElement);
            Assert.NotNull(byteElement);
            Assert.NotNull(shortElement);
            Assert.NotNull(intElement);
            Assert.NotNull(longElement);
            Assert.NotNull(floatElement);
            Assert.NotNull(doubleElement);


            Assert.Equal(TestClass.STRING_VALUE, stringElement.Value);
            Assert.Equal(TestClass.BOOL_VALUE.ToString(), boolElement.Value);
            Assert.Equal(TestClass.BYTE_VALUE.ToString(), byteElement.Value);
            Assert.Equal(TestClass.SHORT_VALUE.ToString(), shortElement.Value);
            Assert.Equal(TestClass.INT_VALUE.ToString(), intElement.Value);
            Assert.Equal(TestClass.LONG_VALUE.ToString(), longElement.Value);
            Assert.Equal(TestClass.FLOAT_VALUE.ToString(), floatElement.Value);
            Assert.Equal(TestClass.DOUBLE_VALUE.ToString(), doubleElement.Value);

        }

        [Fact(DisplayName = "Should convert from xelement to object")]
        public void ShouldConvertFromXElementToObject()
        {
            var converted = INSTANCE.ConvertToObject(XELEMENT);

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
