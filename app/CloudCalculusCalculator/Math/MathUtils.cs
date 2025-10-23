using MathNet.Symbolics;
using Newtonsoft.Json.Linq;

namespace CloudCalculusCalculator.Math
{
    public static class MathUtils
    {
        public static ParsedMath FromJSON(string mathJSON)
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
    }
}
