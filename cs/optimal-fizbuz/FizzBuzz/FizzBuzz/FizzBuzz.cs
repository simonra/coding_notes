using System;
using System.Collections.Generic;
using System.Text;

namespace FizzBuzz
{
    public class FizzBuzz
    {
        public string FizzBuzzForNumber(int number)
        {
            Func<Func<string, string>, Func<string, string>> fi =
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

            Func<Func<string, string>, Func<string, string>> bu =
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

            var result = fi(bu(self))($"{number}");
            return result;

            //var a = Fizz((a, b) => { return $"{a}{b}"; }, number)("", number);
            //return a;
        }

        public Func<string, int, string> Fizz(Func<string, int, string> function, int n)
        {
            if(n % 3 == 0)
            {
                return (word, number) => { return "Fizz" + function("", n); };
            }
            else
            {
                return function;
            }
        }

        public Func<string, int, string> Buzz(Func<string, int, string> function, int n)
        {
            if (n % 5 == 0)
            {
                return (word, number) => { return "Buzz" + function("", n); };
            }
            else
            {
                return function;
            }
        }
    }
}
