using System.Xml.Linq;
using Xunit;

namespace Common.Runtime.Serialization.UnitTests.Xml
{
    using Serialization.Xml;

    public class XSerializerFactoryTest
    {
        [Fact(DisplayName = "Should create instance")]
        public void ShouldCreateInstance()
        {
            var instance = new XSerializerFactory();

            Assert.NotNull(instance);
            Assert.IsAssignableFrom<ISerializerFactory>(instance);
            Assert.IsAssignableFrom<SerializerFactory<XObject>>(instance);
        }
    }
}
