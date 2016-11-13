using Xunit;

namespace MEFSpec
{
    public class MEFSpec
    {
        [Fact]
        public void MEFExportSpec()
        {
            var loader = new Loader();
            loader.Load();

            var cu = loader.Container.GetExport<CalculatorUser2>();

            Assert.NotNull(cu);

            var result = cu.ShowResult("test");
            Assert.Equal<string>("test", result);
        }
    }
}