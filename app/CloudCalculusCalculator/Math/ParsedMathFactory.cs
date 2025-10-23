using Newtonsoft.Json.Linq;

namespace CloudCalculusCalculator.Math
{
    public static class ParsedMathFactory
    {
        public static ParsedMath Create(string json)
        {
            JToken root = JToken.Parse(json);

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
