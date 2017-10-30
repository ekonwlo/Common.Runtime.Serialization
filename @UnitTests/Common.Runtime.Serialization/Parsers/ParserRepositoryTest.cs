using System;
using Xunit;
using NSubstitute;
using Common.Runtime.Serialization.Parsers;

namespace Common.Runtime.Serialization.UnitTests.Parsers
{
    public class ParserRepositoryTest
    {
        private readonly ParserRepository<object> instance = new ParserRepository<object>();
        
        [Fact(DisplayName = "should register")]
        public void ShouldRegister()
        {
            var parser = Substitute.For<IParser<object, string>>();

            instance.Register<string>(parser);

            Assert.Equal(instance, new IParser<object, string>[] { parser });
        }

        [Fact(DisplayName = "should register null parser")]
        public void ShouldNotRegisterNullParser()
        {
            Assert.Throws<ArgumentNullException>( () => { instance.Register<string>(null); } );
        }

        [Fact(DisplayName = "should find")]
        public void ShouldFindParser()
        {
            Assert.Throws<InvalidOperationException>( () => { instance.Find<string>(); } );

            var parser = Substitute.For<IParser<object, string>>();
            instance.Register<string>(parser);

            Assert.Same(parser, instance.Find<string>());
        }
    }
}
