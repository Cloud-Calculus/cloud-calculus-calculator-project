using MathNet.Symbolics;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Numerics;

namespace CloudCalculusCalculator.Math
{
    public abstract class ParsedMath
    {
        public abstract string GetDisplaySolution();
        protected static SymbolicExpression Parse(JToken node)
        {
            return node.Type switch
            {
                JTokenType.Integer => node.Value<long>(),
                JTokenType.Float => node.Value<double>(),
                JTokenType.String => Parse(node.Value<string>()),
                JTokenType.Array => Parse((JArray)node),
                JTokenType.Object or _ => throw new Exception("Unknown type for node.")
            };
        }
        protected static SymbolicExpression Parse(string? value)
        {
            if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double numberValue))
            {
                return numberValue;
            }
            else
            {
                return value switch
                {
                    "ExponentialE" => SymbolicExpression.E,
                    "PositiveInfinity" => SymbolicExpression.PositiveInfinity,
                    "NegativeInfinity" => SymbolicExpression.NegativeInfinity,
                    "Pi" => SymbolicExpression.Pi,
                    "ImaginaryUnit" => SymbolicExpression.I,
                    _ => SymbolicExpression.Variable(value)
                };
            }
        }
        protected static SymbolicExpression Parse(JArray arr)
        {
            string? operation = arr[0].Value<string>();
            SymbolicExpression[] components = new SymbolicExpression[arr.Count - 1];
            for (int i = 1; i < arr.Count; i++)
            {
                components[i - 1] = Parse(arr[i]);
            }

            SymbolicExpression GetProductOfAllTermsAfterFirst()
            {
                SymbolicExpression product = components[1];
                for (int i = 2; i < components.Length; i++)
                {
                    product *= components[i];
                }
                return product;
            }

            SymbolicExpression expression = SymbolicExpression.Zero;
            switch (operation)
            {
                default:
                    throw new FormatException($"Support for operation type \"{operation}\" not yet implemented.");
                case "Negate":
                    expression = -components[0];
                    break;
                case "Add":
                    expression = SymbolicExpression.Sum(components);
                    break;
                case "Subtract":
                    for (int i = 0; i < components.Length; i++)
                    {
                        expression -= components[i]; //More than 2 don't really make sense here anyways
                    }
                    break;
                case "Multiply":
                    expression = SymbolicExpression.Product(components);
                    break;
                case "Rational":
                case "Divide": // there will never be more than 2, but if for some reason there are, get the product of everything after the first
                    expression = components[0] / GetProductOfAllTermsAfterFirst();
                    break;
                case "Power": // there will never be more than 2, but if for some reason there are, get the product of everything after the first
                    expression = components[0].Pow(GetProductOfAllTermsAfterFirst());
                    break;
            }
            return expression;
        }
        protected static string GetDisplaySolutionForVariable(SymbolicExpression lhs, SymbolicExpression rhs, SymbolicExpression variable)
        {
            SymbolicExpression target = (lhs - rhs);
            SymbolicExpression x = variable;

            MathNet.Symbolics.Expression simple = Algebraic.Expand(Rational.Numerator(Rational.Simplify(x.Expression, target.Expression)));

            MathNet.Symbolics.Expression[] coefficients = MathNet.Symbolics.Polynomial.Coefficients(x.Expression, simple);

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
                    MathNet.Symbolics.Expression.Zero.Equals(coefficients[0]) ? target.Expression : MathNet.Symbolics.Expression.Undefined),
                2 => Infix.Format(
                    Rational.Simplify(x.Expression, Algebraic.Expand(-coefficients[0] / coefficients[1]))),
                3 => DisplayIntercepts(
                    new MathNet.Numerics.Polynomial(coefficients.Select(coefficient => (double)Evaluate.Evaluate(null, coefficient).RealValue)).Roots()),
                _ => Infix.Format(
                    MathNet.Symbolics.Expression.Undefined)
            };
        }
    }
}
