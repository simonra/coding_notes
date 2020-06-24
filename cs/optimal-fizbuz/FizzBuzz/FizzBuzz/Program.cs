using System;

namespace FizzBuzz
{
    class Program
    {
        static void Main(string[] args)
        {
            var fb = new FizzBuzz();
            for (int i = 0; i < 36; i++)
            {
                Console.WriteLine(fb.FizzBuzzForNumber(i));
            }
        }
    }
}
