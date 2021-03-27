using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uncategorized;

namespace UnitTests
{
    [TestClass]
    public class IsTrueTests
    {
        [TestMethod]
        public void IsTrue_is_true_when_true()
        {
            // Declare type explicitly, because the extension requires the input to be nullable, and doesn't really make sense if not nullable.
            bool? statement = true;
            if (!statement.IsTrue())
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void IsTrue_is_false_when_false()
        {
            bool? statement = false;
            if (statement.IsTrue())
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void IsTrue_is_false_when_null()
        {
            bool? statement = null;
            if (statement.IsTrue())
            {
                Assert.Fail();
            }
        }
    }
}
