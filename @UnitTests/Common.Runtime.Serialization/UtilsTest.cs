using System;
using Xunit;

namespace Common.Runtime.Serialization.UnitTests
{
    
    public class UtilsTest
    {

        [Fact(DisplayName = "Should get array base type")]
        public void ShouldReturnArrayBaseType()
        {
            Assert.Equal(typeof(string), Utils.GetArrayBaseType(typeof(string[])));
            Assert.Equal(typeof(string), Utils.GetArrayBaseType(typeof(string[][])));
            
            Assert.Equal(typeof(object), Utils.GetArrayBaseType(typeof(object[])));
            Assert.Equal(typeof(object), Utils.GetArrayBaseType(typeof(object[][])));
        }

        [Fact(DisplayName = "Should throw on get array base type when argument is not an array")]
        public void ShouldThrowOnArrayBaseTypeWhenNotArray()
        {
            Assert.Throws<ArgumentException>(() => Utils.GetArrayBaseType(typeof(string)));
            Assert.Throws<ArgumentException>(() => Utils.GetArrayBaseType(typeof(object)));
        }

        [Fact(DisplayName = "Should throw on get array base type when argument is null")]
        public void ShouldThrowOnArrayBaseTypeWhenNull()
        {
            Assert.Throws<ArgumentNullException>(() => Utils.GetArrayBaseType(null));
        }

    }
}
