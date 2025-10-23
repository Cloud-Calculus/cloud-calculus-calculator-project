using MathNet.Symbolics;
using System.Numerics;

namespace CloudCalculusCalculator.Math
{
    public abstract class ParsedMath
    {
        public abstract string GetDisplaySolution();
        protected static string GetDisplaySolutionForVariable(SymbolicExpression lhs, SymbolicExpression rhs, SymbolicExpression variable)
        {
            SymbolicExpression target = (lhs - rhs);
            SymbolicExpression x = variable;

            Expression simple = Algebraic.Expand(Rational.Numerator(Rational.Simplify(x.Expression, target.Expression)));

            Expression[] coefficients = Polynomial.Coefficients(x.Expression, simple);

            string DisplayIntercepts(Complex[] intercepts)
            {
                string result = "";
                for (int i = 0; i < intercepts.Length; i++)
                {
                    string intercept = "";
                    if (intercepts[i].Real != 0 && intercepts[i].Imaginary != 0)
                    {
                        intercept += "(" + intercepts[i].Real + (intercepts[i].Imaginary < 0 ? " " : " + ") + intercepts[i].Imaginary + "i)";
                    }
                    else if (intercepts[i].Real != 0)
                    {
                        intercept += intercepts[i].Real;
                    }
                    else if (intercepts[i].Imaginary != 0)
                    {
                        intercept += intercepts[i].Imaginary + "i";
                    }
                    else
                    {
                        intercept += "0";
                    }
                    result += intercept;
                    if (i + 1 < intercepts.Length)
                    {
                        result += " or ";
                    }
                }
                return result;
            }

            return coefficients.Length switch
            {
                1 => Infix.Format(
                    Expression.Zero.Equals(coefficients[0]) ? target.Expression : Expression.Undefined),
                2 => Infix.Format(
                    Rational.Simplify(x.Expression, Algebraic.Expand(-coefficients[0] / coefficients[1]))),
                3 => DisplayIntercepts(
                    new MathNet.Numerics.Polynomial(coefficients.Select(coefficient => (double)Evaluate.Evaluate(null, coefficient).RealValue)).Roots()),
                _ => Infix.Format(
                    Expression.Undefined)
            };
        }
    }
}
