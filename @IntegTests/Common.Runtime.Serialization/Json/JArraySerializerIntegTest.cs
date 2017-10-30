using Xunit;
using Newtonsoft.Json.Linq;

namespace Common.Runtime.Serialization.IntegTests.Json
{
    using Serialization.Json;
    using Attributes;

    public class JArraySerializerIntegTest
    {
        private static readonly ISerializerFactory FACTORY = new JSerializerFactory();
        private static readonly ISerializer<JToken> INSTANCE = (ISerializer<JToken>) FACTORY.Create<TestPropertyAttribute>(typeof(TestClass))[1];

        [Fact(DisplayName = "Should convert from jarray to array")]
        public void ShouldConvertToArray()
        {
            var array = new JArray(new JArray("r1c1", "r1c2", "r1c3"), new JArray("r2c1", "r2c2", "r2c3"));

            var result = INSTANCE.ConvertToObject(array);

            Assert.Equal(new string[][] { new string[] { "r1c1", "r1c2", "r1c3" }, new string[] { "r2c1", "r2c2", "r2c3" } }, result);
        }

        [Fact(DisplayName = "Should convert from array to jarray")]
        public void ShouldConvertFromArray()
        {
            var array = new string[][] { new string[] { "r1c1", "r1c2", "r1c3" }, new string[] { "r2c1", "r2c2", "r2c3" } };

            var result = INSTANCE.ConvertFromObject(array);

            Assert.Equal(new JArray(new JArray("r1c1", "r1c2", "r1c3"), new JArray("r2c1", "r2c2", "r2c3")), result);
        }

    }
}
