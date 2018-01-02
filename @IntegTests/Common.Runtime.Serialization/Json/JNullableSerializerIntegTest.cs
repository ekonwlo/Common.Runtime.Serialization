using System;
using Xunit;
using Common.Runtime.Serialization.Json;

namespace Common.Runtime.Serialization.IntegTests.Json
{
    using Attributes;

    public class JNullableSerializerIntegTest
    {
        private readonly ISerializer[] _serializers = new JSerializerFactory().Create<TestPropertyAttribute>(typeof(TestNullableClass));
        private readonly ISerializer _stringSerializer;
        private readonly ISerializer _boolSerializer;
        private readonly ISerializer _byteSerializer;
        private readonly ISerializer _shortSerializer;
        private readonly ISerializer _intSerializer;
        private readonly ISerializer _longSerializer;
        private readonly ISerializer _floatSerializer;
        private readonly ISerializer _doubleSerializer;

        // Setup
        public JNullableSerializerIntegTest()
        {
            _stringSerializer = _serializers[0];
            _boolSerializer = _serializers[1];
            _byteSerializer = _serializers[2];
            _shortSerializer = _serializers[3];
            _intSerializer = _serializers[4];
            _longSerializer = _serializers[5];
            _floatSerializer = _serializers[6];
            _doubleSerializer = _serializers[7];
        }

        [Fact(DisplayName = "should parse to string")]
        public void ShouldParseToString()
        {
            Assert.Throws<NotSupportedException>(() => { _boolSerializer.To<string>(null); } );
        }

        [Fact(DisplayName = "should parse from string")]
        public void ShouldParseFromString()
        {
            Assert.Throws<NotSupportedException>(() => { _boolSerializer.From<string>(null); });
        }
    }
}
