using System;
using Xunit;

namespace ExceptionSpec
{
    public class ExceptionSpec
    {

        [Fact]
        public void ThrowTest()
        {
            Assert.ThrowsAsync<Exception>(() =>
            {
                throw new Exception();
            });
        }
    }
}