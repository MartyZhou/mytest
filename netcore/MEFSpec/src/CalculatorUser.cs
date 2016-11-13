using System.Composition;

namespace MEFSpec
{
    [Export]
    public class CalculatorUser
    {

        [Import]
        public ICalculator Calculator { get; set; }

        /*[ImportingConstructor]
        public CalculatorUser(ICalculator calculator)
        {
            this.calculator = calculator;
        }*/

        public string ShowResult(string s)
        {
            return Calculator.calculate(s);
        }
    }

    [Export]
    public class CalculatorUser2
    {
        private readonly ICalculator calculator;

        [ImportingConstructor]
        public CalculatorUser2(ICalculator calculator)
        {
            this.calculator = calculator;
        }

        public string ShowResult(string input)
        {
            return calculator.calculate(input);
        }
    }
}