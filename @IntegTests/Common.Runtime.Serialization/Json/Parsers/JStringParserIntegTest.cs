using System;
using Newtonsoft.Json.Linq;
using Xunit;
using Common.Runtime.Serialization.Json.Parsers;

namespace Common.Runtime.Serialization.IntegTests.Json.Parsers
{
    public class JStringParserIntegTest
    {
        private readonly JStringParser instance = new JStringParser();

        [Fact(DisplayName = "should parse from string")]
        public void ShouldParseFrom()
        {
            var result = instance.ParseFrom("{ prop1: 1, prop2: \"val2\" }");

            Assert.NotNull(result);
            Assert.IsAssignableFrom<JToken>(result);
        }

        [Fact(DisplayName = "should parse to string")]
        public void ShouldParseTo()
        {
            var result = instance.ParseTo(new JProperty("prop1", new JValue("val1")));

            Assert.NotNull(result);
        }

        [Fact(DisplayName = "should throw on parse from string")]
        public void ShouldThrowOnParseFrom()
        {
            Assert.Throws<ArgumentNullException>( () => { instance.ParseFrom(null); });
        }

        [Fact(DisplayName = "should throw on parse to string")]
        public void ShouldThrowOnParseTo()
        {
            Assert.Throws<ArgumentNullException>(() => { instance.ParseTo(null); });
        }
    }
}
