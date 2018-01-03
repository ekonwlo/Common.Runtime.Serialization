using System;
using System.Globalization;
using Xunit;
using Common.Runtime.Serialization.Xml;

namespace Common.Runtime.Serialization.IntegTests.Xml
{
    using Attributes;

    public class XPrimitiveSerializerIntegTest
    {
        private readonly ISerializer[] _serializers = new XSerializerFactory().Create<TestPropertyAttribute>(typeof(TestChildClass));
        private readonly ISerializer _stringSerializer;
        private readonly ISerializer _boolSerializer;
        private readonly ISerializer _byteSerializer;
        private readonly ISerializer _shortSerializer;
        private readonly ISerializer _intSerializer;
        private readonly ISerializer _longSerializer;
        private readonly ISerializer _floatSerializer;
        private readonly ISerializer _doubleSerializer;

        // Setup
        public XPrimitiveSerializerIntegTest()
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

        [Fact(DisplayName = "should set property")]
        public void ShouldSetProperty()
        {
            var o = new TestChildClass();

            _stringSerializer.SetProperty<string>(o, "other_value");
            _boolSerializer.SetProperty<string>(o, "false");
            _byteSerializer.SetProperty<string>(o, "128");
            _shortSerializer.SetProperty<string>(o, "1300");
            _intSerializer.SetProperty<string>(o, "12345678");
            _longSerializer.SetProperty<string>(o, "1234567981234456789");
            _floatSerializer.SetProperty<string>(o, "2345");
            _doubleSerializer.SetProperty<string>(o, "3454575667868");

            Assert.Equal("other_value", o.StringParam);
            Assert.Equal(false, o.BoolParam);
            Assert.Equal(128, o.ByteParam);
            Assert.Equal(1300, o.ShortParam);
            Assert.Equal(12345678, o.IntParam);
            Assert.Equal(1234567981234456789, o.LongParam);
            Assert.Equal(2345f, o.FloatParam);
            Assert.Equal(3454575667868d, o.DoubleParam);
        }

        [Fact(DisplayName = "should get property")]
        public void ShouldGetProperty()
        {
            var o = new TestChildClass();

            Assert.Equal("value", _stringSerializer.GetProperty<string>(o));
            Assert.Equal("True", _boolSerializer.GetProperty<string>(o));
            Assert.Equal("64", _byteSerializer.GetProperty<string>(o));
            Assert.Equal("16000", _shortSerializer.GetProperty<string>(o));
            Assert.Equal("15", _intSerializer.GetProperty<string>(o));
            Assert.Equal("32500000", _longSerializer.GetProperty<string>(o));
            Assert.Equal("164564", _floatSerializer.GetProperty<string>(o));
            Assert.Equal("16675567567557", _doubleSerializer.GetProperty<string>(o));
        }

        [Fact(DisplayName = "should parse to string")]
        public void ShouldParseToString()
        {
            Assert.Null(_stringSerializer.To<string>(null));
            Assert.Equal("abc", _stringSerializer.To<string>("abc"));
            Assert.Equal("False", _boolSerializer.To<string>(false));
            Assert.Equal("255", _byteSerializer.To<string>(Byte.MaxValue));
            Assert.Equal("32767", _shortSerializer.To<string>(Int16.MaxValue));
            Assert.Equal("2147483647", _intSerializer.To<string>(Int32.MaxValue));
            Assert.Equal(Int64.MaxValue.ToString(), _longSerializer.To<string>(Int64.MaxValue));
            Assert.Equal(String.Format(CultureInfo.GetCultureInfo("en-GB"), "{0:0}", 200f)
               , _floatSerializer.To<string>(200f));
            Assert.Equal(String.Format(CultureInfo.GetCultureInfo("en-GB"), "{0:0}", 400d)
                , _doubleSerializer.To<string>(400d));
            // ISSUE with large numbers, Newtonsoft.Json is using sceintific notation
            //Assert.Equal(String.Format(CultureInfo.GetCultureInfo("en-GB"), "{0:0.0}", 4568767868f)
            //    , _floatSerializer.To<string>(4568767868f));
            //Assert.Equal(String.Format(CultureInfo.GetCultureInfo("en-GB"), "{0:0.0}", 456876786869894d)
            //    , _doubleSerializer.To<string>(456876786869894d));
        }

        [Fact(DisplayName = "should throw on parsing to string")]
        public void ShouldThrowOnParsingToString()
        {
            var o = new { prop1 = 1, prop2 = 2 };

            Assert.Throws<SerializationException>(() => { _stringSerializer.To<string>(o); });
        }

        [Fact(DisplayName = "should parse from string")]
        public void ShouldParseFromString()
        {
            Assert.Equal("abc", _stringSerializer.From<string>("abc"));
            Assert.Equal(true, _boolSerializer.From<string>("true"));
            Assert.Equal(Byte.MaxValue, _byteSerializer.From<string>(Byte.MaxValue.ToString()));
            Assert.Equal(Int16.MaxValue, _shortSerializer.From<string>(Int16.MaxValue.ToString()));
            Assert.Equal(Int32.MaxValue, _intSerializer.From<string>(Int32.MaxValue.ToString()));
            Assert.Equal(Int64.MaxValue, _longSerializer.From<string>(Int64.MaxValue.ToString()));
            Assert.Equal(4568767868f, _floatSerializer.From<string>(String.Format(CultureInfo.GetCultureInfo("en-GB"), "{0:0}", 4568767868f)));
            Assert.Equal(456876786869894d, _doubleSerializer.From<string>(String.Format(CultureInfo.GetCultureInfo("en-GB"), "{0:0}", 456876786869894d)));
        }

        [Fact(DisplayName = "should throw when parsing from string")]
        public void ShouldThrowOnParsingFromString()
        {
            Assert.Throws<ArgumentNullException>(() => { _stringSerializer.From<string>(null); });

            Assert.Throws<SerializationException>(() => { _stringSerializer.From<string>("<abc>"); });
            Assert.Throws<SerializationException>(() => { _stringSerializer.From<string>("&abc"); });
            Assert.Throws<SerializationException>(() => { _boolSerializer.From<string>("\"invalid\""); });
            Assert.Throws<SerializationException>(() => { _byteSerializer.From<string>("\"invalid\""); });
            Assert.Throws<SerializationException>(() => { _byteSerializer.From<string>("256"); });
        }
    }
}
