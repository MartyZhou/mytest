using Xunit;

namespace netcore.BuiltInTypes
{
    public class StringSpec
    {
        [Fact]
        public void StringDeclare()
        {
            string string1 = "Hello";

            Assert.Equal("Hello", string1);
        }

        [Fact]
        public void StringEmpty()
        {
            string empty = string.Empty;

            Assert.Equal("", empty);
        }

        [Fact]
        public void StringUpperCase()
        {
            string test = "abCd".ToUpper();

            Assert.Equal("ABCD", test);
        }

        [Fact]
        public void StringTrim()
        {
            string test = " marty  ".Trim();

            Assert.Equal("marty", test);
        }

        [Fact]
        public void StringTrim2()
        {
            string test = "__marty--".Trim('_', '-');

            Assert.Equal("marty", test);
        }
    }
}