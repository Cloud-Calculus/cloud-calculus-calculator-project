using MathNet.Symbolics;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace CloudCalculusCalculator.Math
{
    public static class MathUtils
    {
        public static ParsedMath ParsedMathFactory(string mathJSON)
        {
            JToken root = JToken.Parse(mathJSON);

            if (root is not JArray arr)
            {
                throw new FormatException("Input was not an array like expected.");
            }

            return arr[0].Value<string>() switch
            {
                "Equal" => new ParsedEquality(arr),
                _ => new ParsedExpression(arr)
            };
        }
        public static SymbolicExpression Parse(JToken node)
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
        public static SymbolicExpression Parse(string? value)
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
        public static SymbolicExpression Parse(JArray arr)
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
    }
}
