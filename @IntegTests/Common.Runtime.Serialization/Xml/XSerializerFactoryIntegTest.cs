﻿using System.Collections.Generic;
using System.Xml.Linq;
using Xunit;

namespace Common.Runtime.Serialization.IntegTests.Xml
{
    using Attributes;
    using Serialization.Xml;

    public class XSerializerFactoryIntegTest
    {        
        [Fact(DisplayName = "Should create converters")]
        public void ShouldCreateConverter()
        {
            var instance = new XSerializerFactory();
            var serializers = instance.Create<TestPropertyAttribute>(typeof(TestClass));

            Assert.IsAssignableFrom<IEnumerable<ISerializer<XObject>>>(serializers);
        }

    }
}
