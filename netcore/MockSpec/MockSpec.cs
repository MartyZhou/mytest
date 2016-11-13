using Xunit;
using Moq;

namespace MockSpec
{
    public class MockSpec
    {
        [Fact]
        public void MockSpec1()
        {
            Mock<IFoo> mockFoo = new Mock<IFoo>();
            mockFoo.Setup(f => f.DoSomething(It.IsAny<string>())).Returns(true);

            var result = mockFoo.Object.DoSomething("anything");

            Assert.True(result);
        }
    }

    public interface IFoo
    {
        bool DoSomething(string s);
        string DoSomethingStringy(string s);
        bool TryParse(string s, out string s2);
        bool Submit(ref object o);
        int GetCount();
        int GetCountThing();
    }
}