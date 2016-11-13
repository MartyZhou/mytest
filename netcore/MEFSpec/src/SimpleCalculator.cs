using System.Composition;

namespace MEFSpec
{
    [Export(typeof(ICalculator))]
    public class SimpleCalculator : ICalculator
    {
        public string calculate(string input)
        {
            return input;
        }
    }

    /*[Export(typeof(ICalculator))]
    public class SimpleCalculator2 : ICalculator
    {
        public string calculate(string input)
        {
            return input;
        }
    }*/
}