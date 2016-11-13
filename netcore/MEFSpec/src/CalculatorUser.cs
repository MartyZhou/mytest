using System.Composition;

namespace MEFSpec
{
    public class CalculatorUser
    {

        private readonly ICalculator calculator;

        [ImportingConstructor]
        public CalculatorUser(ICalculator calculator)
        {
            this.calculator = calculator;
        }

        public string ShowResult(string s)
        {
            return calculator.calculate(s);
        }
    }
}