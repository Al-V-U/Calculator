using System.Numerics;
using JetBrains.Annotations;

namespace _Scripts.Services.Calculator
{
    public class CalculateService : ICalculateService
    {
        private const string Error = " = ERROR";

        public (string, bool) AddCalculate([NotNull] string equation)
        {
            equation = RemoveSpaces(equation);
            string[] members = SplitStringByDelimiter(equation, '+');

            if (members.Length != 2)
                return (ReturnError(equation), false);

            foreach (var member in members)
            {
                if (!IsOnlyDigits(member))
                    return (ReturnError(equation), false);
            }

            return ($"{members[0]} + {members[1]} = {AddBigNumbers(members[0], members[1])}", true);
        }
        
        private static string RemoveSpaces(string equation)
        {
            return equation.Replace(" ", "");
        }
        
        private static bool IsOnlyDigits(string member)
        {
            if (string.IsNullOrWhiteSpace(member))
                return false;
            
            foreach (char c in member)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }
        private static string[] SplitStringByDelimiter(string input, char delimiter)
        {
            return input.Split(delimiter);
        }

        private static string ReturnError(string equation)
        {
            return equation + Error;
        }
        
        private static BigInteger AddBigNumbers(string num1, string num2)
        {
            BigInteger number1 = BigInteger.Parse(num1);
            BigInteger number2 = BigInteger.Parse(num2);
        
            return number1 + number2;
        }
    }
}
