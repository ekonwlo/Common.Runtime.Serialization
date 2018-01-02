using System;
using Xunit;
using Newtonsoft.Json.Linq;

namespace Common.Runtime.Serialization.IntegTests.Json
{
    using Serialization.Json;
    using Attributes;

    public class JArraySerializerIntegTest
    {
        private static JArray _jStringArray = new JArray(new JArray("r1c1", "r1c2", "r1c3"), new JArray("r2c1", "r2c2", "r2c3"));
        private static string[][] _stringArray = new string[][] { new string[] { "r1c1", "r1c2", "r1c3" }, new string[] { "r2c1", "r2c2", "r2c3" } };

        private readonly ISerializer<JToken> _instance = (ISerializer<JToken>) new JSerializerFactory().Create<TestPropertyAttribute>(typeof(TestClass))[1];

        [Fact(DisplayName = "Should convert from jarray to array")]
        public void ShouldConvertToArray()
        {
            Assert.Equal(_stringArray, _instance.ConvertToObject(_jStringArray));
        }

        [Fact(DisplayName = "Should convert from array to jarray")]
        public void ShouldConvertFromArray()
        {
            Assert.Equal(_jStringArray, _instance.ConvertFromObject(_stringArray));
        }

        [Fact(DisplayName = "should set property")]
        public void ShouldSetProperty()
        {
            var o = new TestClass();

            _instance.SetProperty<string>(o, "[[\"r1c1\",\"r1c2\",\"r1c3\"]]");
           
            Assert.Equal(new string[][] { new string[] { "r1c1", "r1c2", "r1c3" } }, o.StringArray);
        }

        [Fact(DisplayName = "should get property")]
        public void ShouldGetProperty()
        {
            var o = new TestClass();

            Assert.Equal("[[\"r1c1\",\"r1c2\",\"r1c3\"],[\"r2c1\",\"r2c2\",\"r2c3\"]]", _instance.GetProperty<string>(o));
        }

        [Fact(DisplayName = "should parse to string")]
        public void ShouldParserToString()
        {
            Assert.Equal("null", _instance.To<string>(null));
            Assert.Equal("[[\"r1c1\",\"r1c2\",\"r1c3\"],[\"r2c1\",\"r2c2\",\"r2c3\"]]", _instance.To<string>(_stringArray));
        }

        [Fact(DisplayName = "should throw on parsing to string")]
        public void ShouldThrowOnParsingToString()
        {
            var o = new { prop1 = 1, prop2 = 2 };

            Assert.Throws<SerializationException>(() => { _instance.To<string>(o); });
        }

        [Fact(DisplayName = "should parse from string")]
        public void ShouldParserFromString()
        {
            Assert.Null(_instance.From<string>("null"));
            Assert.Equal(_stringArray, _instance.From<string>("[[\"r1c1\",\"r1c2\",\"r1c3\"],[\"r2c1\",\"r2c2\",\"r2c3\"]]"));
        }


        [Fact(DisplayName = "should throw when parsing from string")]
        public void ShouldThrowOnParsingFromString()
        {
            Assert.Throws<ArgumentNullException>(() => { _instance.From<string>(null); });

            Assert.Throws<SerializationException>(() => { _instance.From<string>("\"invalid\""); });
            Assert.Throws<SerializationException>(() => { _instance.From<string>("256"); });
        }
    }
}
