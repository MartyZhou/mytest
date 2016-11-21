using System;
using System.Threading.Tasks;
using Xunit;

namespace ExceptionSpec
{
    public class ExceptionSpec
    {

        [Fact]
        public void ThrowTest()
        {
            Assert.ThrowsAny<Exception>(() =>
            {
                throw new Exception();
            });
        }
    }
}