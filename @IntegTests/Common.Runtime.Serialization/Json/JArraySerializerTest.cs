using Xunit;
using Newtonsoft.Json.Linq;

namespace Common.Runtime.Serialization.IntegTests.Json
{
    using Serialization.Json;
    using Attributes;

    public class JArraySerializerTest
    {
        private static readonly ISerializerFactory<JToken> FACTORY = new JSerializerFactory();
        private static readonly ISerializer<JToken> INSTANCE = FACTORY.Create<TestPropertyAttribute>(typeof(TestClass))[1];

        [Fact]
        public void ShouldConvertToArray()
        {
            var o = new TestClass();

            var array = new JArray(new JArray("r1c1", "r1c2", "r1c3"), new JArray("r2c1", "r2c2", "r2c3"));

            //INSTANCE.ConvertToObject(array);

            var result = INSTANCE.ConvertFromObject(o);



        }

    }
}
