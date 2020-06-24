using System;

namespace FizzBuzz
{
    class Program
    {
        static void Main(string[] args)
        {
            var fb = new FizzBuzzLibrary();
            for (int i = 0; i < 36; i++)
            {
                Console.WriteLine(fb.FizzBuzz(i));
            }
        }
    }
}
