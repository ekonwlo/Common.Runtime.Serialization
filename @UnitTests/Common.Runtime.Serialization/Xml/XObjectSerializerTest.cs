using System;
using System.Xml.Linq;
using System.Reflection;
using NSubstitute;
using Xunit;

namespace Common.Runtime.Serialization.UnitTests.Xml
{
    using Attributes;
    using Serialization.Attributes;
    using Serialization.Xml;

    public class XObjectSerializerTest
    {
        private static readonly SerializerFactory<XObject> FACTORY = Substitute.For<SerializerFactory<XObject>>();
        private static readonly Type TEST_TYPE = typeof(TestClass);
        private static readonly PropertyInfo STRING_PROP = TEST_TYPE.GetProperty("StringParam");
        private static readonly ISerializableProperty STRING_ATTR = Substitute.For<ISerializableProperty>();
        private static readonly ConstructorInfo CONSTRUCTOR = TEST_TYPE.GetConstructor(Type.EmptyTypes);
        
        static XObjectSerializerTest()
        {
            STRING_ATTR.Name.Returns(TestClass.STRING_ATTR_NAME);
        }

        [Fact(DisplayName = "Should create instance")]
        public void ShouldCreateInstance()
        {
            var instance = new XObjectSerializer(FACTORY, TEST_TYPE, STRING_PROP, STRING_ATTR, null, null, CONSTRUCTOR, FACTORY.CreateSerializsers<TestPropertyAttribute>);

            Assert.IsAssignableFrom<ISerializer<XObject>>(instance);
            Assert.Equal(TEST_TYPE, instance.Type);
        }

        [Fact(DisplayName = "Should throw on invalid arguments")]
        public void ShouldThrowOnInvalidArguments()
        {
            Assert.Throws<ArgumentNullException>(() => { new XObjectSerializer(null, TEST_TYPE, STRING_PROP, STRING_ATTR, null, null, CONSTRUCTOR, FACTORY.CreateSerializsers<TestPropertyAttribute>); });
            Assert.Throws<ArgumentNullException>(() => { new XObjectSerializer(FACTORY, null, STRING_PROP, STRING_ATTR, null, null, CONSTRUCTOR, FACTORY.CreateSerializsers<TestPropertyAttribute>); });
            Assert.Throws<ArgumentNullException>(() => { new XObjectSerializer(FACTORY, TEST_TYPE, null, STRING_ATTR, null, null, CONSTRUCTOR, FACTORY.CreateSerializsers<TestPropertyAttribute>); });
            Assert.Throws<ArgumentNullException>(() => { new XObjectSerializer(FACTORY, TEST_TYPE, STRING_PROP, null, null, null, CONSTRUCTOR, FACTORY.CreateSerializsers<TestPropertyAttribute>); });
        }

        [Fact(DisplayName = "Should convert from object")]
        public void SouldConvertFromObject()
        {
            var instance = new XObjectSerializer(FACTORY, TEST_TYPE, STRING_PROP, STRING_ATTR, null, null, CONSTRUCTOR, FACTORY.CreateSerializsers<TestPropertyAttribute>);

            var o = new TestClass();

            var converted = instance.ConvertFromObject(o);

            Assert.NotNull(converted);
        }

    }
}
