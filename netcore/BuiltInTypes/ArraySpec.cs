using Xunit;

namespace netcore.BuiltInTypes
{
    public class ArraySpec
    {
        [Fact]
        public void ArrayDeclare()
        {
            int[] testArray = new int[] { 1, 2, 3 };
            Assert.Equal(3, testArray.Length);
        }

        [Fact]
        public void ArrayDeclare2D()
        {
            int[,] testArray = { { 1, 3, 3 }, { 2, 3, 3 } };

            Assert.Equal(6, testArray.Length);
        }
    }
}