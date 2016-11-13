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
        }
    }
}