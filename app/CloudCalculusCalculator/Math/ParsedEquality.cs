using MathNet.Symbolics;
using Newtonsoft.Json.Linq;

namespace CloudCalculusCalculator.Math
{
    public class ParsedEquality : ParsedMath
    {
        private SymbolicExpression lhs, rhs;
        public ParsedEquality(JArray arr)
        {
            if (arr.Count != 3)
            {
                throw new Exception("No relation defined for equation with total terms not equal to 2.");
            }
            lhs = MathUtils.Parse(arr[1]);
            rhs = MathUtils.Parse(arr[2]);
        }
        public override string GetDisplaySolution() => GetDisplaySolutionForVariable(lhs, rhs, SymbolicExpression.Variable("x"));
    }
}
