using System.Composition.Convention;
using System.Composition.Hosting;
using System.Reflection;

namespace MEFSpec
{
    public class Loader
    {
        private CompositionHost container;

        public CompositionHost Container
        {
            get { return container; }
        }

        public void Load()
        {
            var conventions = new ConventionBuilder();
            //conventions.ForType<CalculatorUser>()
            //.ImportProperty<ICalculator>(p => p.Calculator)
            //.Export();
            //conventions.ForType<ICalculator>()
            //.Export<ICalculator>()
            //.Shared();

            var assemblies = new[] { typeof(SimpleCalculator).GetTypeInfo().Assembly };

            var configuration = new ContainerConfiguration().WithAssemblies(assemblies, conventions);

            var assembly = typeof(Loader).GetTypeInfo().Assembly;
            var configuration2 = new ContainerConfiguration().WithAssembly(assembly);

            container = configuration2.CreateContainer();
        }
    }
}