using System;

namespace FizzBuzz
{
    public class Program
    {
        public static void Main(string[] args)
        {
            for (int i = 0; i < 36; i++)
            {
                Console.WriteLine(FizzBuzzLibrary.FizzBuzz(i));
            }
        }
    }
}
