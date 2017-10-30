using Newtonsoft.Json.Linq;
using Xunit;


namespace Common.Runtime.Serialization.UnitTests.Json
{
    using Serialization.Json;

    public class JSerializerFactoryTest
    {
        [Fact(DisplayName = "Should create instance")]
        public void ShouldCreateInstance()
        {
            var instance = new JSerializerFactory();

            Assert.NotNull(instance);
            Assert.IsAssignableFrom<ISerializerFactory>(instance);
            Assert.IsAssignableFrom<SerializerFactory<JToken>>(instance);
        }
    }
}
