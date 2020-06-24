using System;

namespace FizzBuzz
{
    public static class FizzBuzzLibrary
    {
        public static string FizzBuzz(int number)
        {
            Func<Func<string, string>, Func<string, string>> fizz =
                f =>
                {
                    if(number % 3 == 0)
                    {
                        return x => "Fizz" + f("");
                    }
                    else
                    {
                        return f;
                    }
                };

            Func<Func<string, string>, Func<string, string>> buzz =
                f =>
                {
                    if (number % 5 == 0)
                    {
                        return x => "Buzz" + f("");
                    }
                    else
                    {
                        return f;
                    }
                };

            Func<string, string> self = x => x;

            var result = fizz(buzz(self))($"{number}");
            return result;
        }
    }
}
