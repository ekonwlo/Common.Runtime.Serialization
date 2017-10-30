using System;
using System.Xml.Linq;
using Xunit;
using Common.Runtime.Serialization.Xml.Parsers;

namespace Common.Runtime.Serialization.IntegTests.Xml.Parsers
{
    public class XStringParserIntegTests
    {
        private readonly XStringParser instance = new XStringParser();

        [Fact(DisplayName = "should parse from string")]
        public void ShouldParseFrom()
        {
            var result = instance.ParseFrom("<xml></xml>");

            Assert.NotNull(result);
            Assert.IsAssignableFrom<XObject>(result);
        }

        [Fact(DisplayName = "should parse to string")]
        public void ShouldParseTo()
        {
            var result = instance.ParseTo(new XElement("prop1" , new XText("val1")));

            Assert.NotNull(result);
        }

        [Fact(DisplayName = "should throw on parse from string")]
        public void ShouldThrowOnParseFrom()
        {
            Assert.Throws<ArgumentNullException>(() => { instance.ParseFrom(null); });
        }

        [Fact(DisplayName = "should throw on parse to string")]
        public void ShouldThrowOnParseTo()
        {
            Assert.Throws<ArgumentNullException>(() => { instance.ParseTo(null); });
        }
    }
}
