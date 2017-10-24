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

    public class JArraySerializerTest
    {
        private static readonly SerializerFactory<JToken> FACTORY = Substitute.For<SerializerFactory<JToken>>();

        private static readonly Type TEST_TYPE = typeof(TestClass);

        private static readonly PropertyInfo ARRAY_PROP = TEST_TYPE.GetProperty("ArrayStringParam");
        //private static readonly PropertyInfo ARRAY_PROP = Substitute.For<PropertyInfo>();
        private static readonly ISerializableProperty ARRAY_ATTR = Substitute.For<ISerializableProperty>();
        private static readonly ISerializer<JToken> BASE_SERIALIZER = Substitute.For<ISerializer<JToken>>();

        static JArraySerializerTest()
        {
            //ARRAY_PROP.DeclaringType.Returns(typeof(TestClass));
            //ARRAY_PROP.PropertyType.Returns(typeof(string[][]));
            BASE_SERIALIZER.GetElementValue(new JValue("r1c1")).Returns("r1c1");
            BASE_SERIALIZER.GetElementValue(new JValue("r1c2")).Returns("r1c2");
            BASE_SERIALIZER.GetElementValue(new JValue("r1c3")).Returns("r1c3");
            BASE_SERIALIZER.GetElementValue(new JValue("r2c1")).Returns("r2c1");
            BASE_SERIALIZER.GetElementValue(new JValue("r2c2")).Returns("r2c2");
            BASE_SERIALIZER.GetElementValue(new JValue("r2c3")).Returns("r2c3");
            BASE_SERIALIZER.SetElementValue("r1c1").Returns(new JValue("r1c1"));
            BASE_SERIALIZER.SetElementValue("r1c2").Returns(new JValue("r1c2"));
            BASE_SERIALIZER.SetElementValue("r1c3").Returns(new JValue("r1c3"));
            BASE_SERIALIZER.SetElementValue("r2c1").Returns(new JValue("r2c1"));
            BASE_SERIALIZER.SetElementValue("r2c2").Returns(new JValue("r2c2"));
            BASE_SERIALIZER.SetElementValue("r2c3").Returns(new JValue("r2c3"));

        }

        [Fact(DisplayName = "Should create instance")]
        public void ShouldCreateInstance() 
        {
            var instance = new JArraySerializer(FACTORY, typeof(string[][]), ARRAY_PROP, ARRAY_ATTR, null, null, BASE_SERIALIZER);

            Assert.NotNull(instance);
            Assert.IsAssignableFrom<ISerializer<JToken>>(instance);
            Assert.Equal(TypeDefinition.Of<string[][]>(), instance.Type);
            Assert.Equal(TypeDefinition.StringType, instance.ElementType);
            Assert.Equal(ARRAY_ATTR, instance.Attribute);
            Assert.Equal(ARRAY_PROP, instance.Property);
            Assert.Equal(2, instance.Dimiensions);

        }

        [Fact(DisplayName = "Should throw on invalid arguments")]
        public void ShouldThrowOnInvalidArguments()
        {
            Assert.Throws<ArgumentNullException>(() => { new JArraySerializer(null, typeof(string[][]), ARRAY_PROP, ARRAY_ATTR, null, null, BASE_SERIALIZER); });
            Assert.Throws<ArgumentNullException>(() => { new JArraySerializer(FACTORY, null, ARRAY_PROP, ARRAY_ATTR, null, null, BASE_SERIALIZER); });
            Assert.Throws<ArgumentNullException>(() => { new JArraySerializer(FACTORY, typeof(string[][]), null, ARRAY_ATTR, null, null, BASE_SERIALIZER); });
            Assert.Throws<ArgumentNullException>(() => { new JArraySerializer(FACTORY, typeof(string[][]), ARRAY_PROP, null, null, null, BASE_SERIALIZER); });
            Assert.Throws<ArgumentNullException>(() => { new JArraySerializer(FACTORY, typeof(string[][]), ARRAY_PROP, ARRAY_ATTR, null, null, null); });

            ISerializableProperty array_attr = Substitute.For<ISerializableProperty>();
            array_attr.Name.Returns((string)null);
            Assert.Throws<ArgumentNullException>(() => { new JArraySerializer(FACTORY, typeof(string[][]), ARRAY_PROP, array_attr, null, null, BASE_SERIALIZER); });
        }

        [Fact(DisplayName = "Should convert jarray to string array")]
        public void ShouldConvertJArrayToArray()
        {
            var array = new JArray(new JArray("r1c1", "r1c2", "r1c3"), new JArray("r2c1", "r2c2", "r2c3"));

            var instance = new JArraySerializer(FACTORY, typeof(string[][]), ARRAY_PROP, ARRAY_ATTR, null, null, BASE_SERIALIZER);

            var result = instance.ConvertToObject(array) as string[][];

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal("r1c1", result[0][0]);
            Assert.Equal("r1c2", result[0][1]);
            Assert.Equal("r1c3", result[0][2]);
            Assert.Equal("r2c1", result[1][0]);
            Assert.Equal("r2c2", result[1][1]);
            Assert.Equal("r2c3", result[1][2]);
        }

        [Fact(DisplayName = "Should convert string array to jarray")]
        public void ShouldConvertStringArrayToJAray()
        {   
            var array = new string[][] { new string[] { "r1c1", "r1c2", "r1c3" }, new string[] { "r2c1", "r2c2", "r2c3" } };

            var instance = new JArraySerializer(FACTORY, typeof(string[][]), ARRAY_PROP, ARRAY_ATTR, null, null, BASE_SERIALIZER);

            var result = instance.ConvertFromObject(array);

            Assert.NotNull(result);
            Assert.Equal("r1c1", result[0][0]);
            Assert.Equal("r1c2", result[0][1]);
            Assert.Equal("r1c3", result[0][2]);
            Assert.Equal("r2c1", result[1][0]);
            Assert.Equal("r2c2", result[1][1]);
            Assert.Equal("r2c3", result[1][2]);
        }

    }
}
