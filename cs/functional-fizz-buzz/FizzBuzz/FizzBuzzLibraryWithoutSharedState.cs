using System;

namespace FizzBuzz
{
    public static class FizzBuzzLibraryWithoutSharedState
    {
        public static string FizzBuzz(int number)
        {
            return Fizz(Buzz(BounceString, number), number)($"{number}", number);
        }

        private static Func<string, int, string> Fizz(Func<string, int, string> wrappedFunction, int number)
        {
            if(number % 3 == 0)
            {
                return (strParam, intParam) => { return "Fizz" + wrappedFunction("", number); };
            }
            else
            {
                return wrappedFunction;
            }
        }

        private static Func<string, int, string> Buzz(Func<string, int, string> wrappedFunction, int number)
        {
            if (number % 5 == 0)
            {
                return (strParam, intParam) => { return "Buzz" + wrappedFunction("", number); };
            }
            else
            {
                return wrappedFunction;
            }
        }

        private static string BounceString(string desiredOutput, int number)
        {
            return desiredOutput;
        }
    }
}
