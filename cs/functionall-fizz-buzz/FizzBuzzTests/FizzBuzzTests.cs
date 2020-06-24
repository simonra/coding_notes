using NUnit.Framework;

namespace FizzBuzz
{
    public class FizzBuzzTests
    {
        private int number;
        private string result;

        private FizzBuzzTests Given => this;
        private FizzBuzzTests When => this;
        private FizzBuzzTests Then => this;

        [Test]
        public void FizzBuzz_of_number_divisible_by_3_but_not_5_should_return_Fizz()
        {
            Given.We_have_a_number_divisible_by_3_but_not_5();
            When.We_do_FizzBuzz_of_number();
            Then.Result_is_Fizz();
        }

        [Test]
        public void FizzBuzz_of_number_divisible_by_5_but_not_3_should_return_Buzz()
        {
            Given.We_have_a_number_divisible_by_5_but_not_3();
            When.We_do_FizzBuzz_of_number();
            Then.Result_is_Buzz();
        }

        [Test]
        public void FizzBuzz_of_number_divisible_by_both_3_and_5_should_return_FizzBuzz()
        {
            Given.We_have_a_number_divisible_by_3_and_5();
            When.We_do_FizzBuzz_of_number();
            Then.Result_is_FizzBuzz();
        }

        [Test]
        public void FizzBuzz_of_number_neither_divisible_by_3_nor_5_should_return_number()
        {
            Given.We_have_a_number_netiher_divisible_by_3_nor_5();
            When.We_do_FizzBuzz_of_number();
            Then.Result_is_same_as_number();
        }

        private void We_have_a_number_divisible_by_3_but_not_5()
        {
            number = 2 * 3 * 4 * 6 * 7;
        }

        private void We_have_a_number_divisible_by_5_but_not_3()
        {
            number = 2 * 4 * 5 * 7 * 8;
        }

        private void We_have_a_number_divisible_by_3_and_5()
        {
            number = 2 * 3 * 4 * 5 * 6 * 7 * 8;
        }

        private void We_have_a_number_netiher_divisible_by_3_nor_5()
        {
            number = 2 * 4 * 7 * 8 * 11 * 13;
        }

        private void We_do_FizzBuzz_of_number()
        {
            result = FizzBuzzLibrary.FizzBuzz(number);
        }

        private void Result_is_Fizz()
        {
            Assert.AreEqual("Fizz", result);
        }

        private void Result_is_Buzz()
        {
            Assert.AreEqual("Buzz", result);
        }

        private void Result_is_FizzBuzz()
        {
            Assert.AreEqual("FizzBuzz", result);
        }

        private void Result_is_same_as_number()
        {
            Assert.AreEqual($"{number}", result);
        }
    }
}
