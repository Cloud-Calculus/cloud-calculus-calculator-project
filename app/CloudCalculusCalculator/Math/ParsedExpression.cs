using MathNet.Symbolics;
using Newtonsoft.Json.Linq;

namespace CloudCalculusCalculator.Math
{
    public class ParsedExpression : ParsedMath
    {
        private SymbolicExpression expression;
        public ParsedExpression(JArray arr)
        {
            expression = MathUtils.Parse(arr);
        }
        public override string GetDisplaySolution() => GetDisplaySolutionForVariable(expression, SymbolicExpression.Zero, SymbolicExpression.Variable("x"));
    }
}
