using System.Composition.Convention;
using System.Composition.Hosting;
using System.Reflection;

namespace MEFSpec
{
    public class Loader
    {
        public void Load()
        {
            var conventions = new ConventionBuilder();
            conventions.ForType<ICalculator>()
            .Export<ICalculator>()
            .Shared();

            var assemblies = new[] { typeof(SimpleCalculator).GetTypeInfo().Assembly };

            var configuration = new ContainerConfiguration().WithAssemblies(assemblies, conventions);

            using (var container = configuration.CreateContainer())
            {
                var plugins = container.GetExports<ICalculator>();
            }
        }
    }
}