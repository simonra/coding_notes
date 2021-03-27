using Microsoft.VisualStudio.TestTools.UnitTesting;
using UncategorizedExtensions;

namespace UnitTests
{
    [TestClass]
    public class IsFalseTests
    {
        [TestMethod]
        public void IsFalse_is_true_when_false()
        {
            // Declare type explicitly, because the extension requires the input to be nullable, and doesn't really make sense if not nullable.
            bool? statement = false;
            if (!statement.IsFalse())
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void IsFalse_is_false_when_true()
        {
            bool? statement = true;
            if (statement.IsFalse())
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void IsFalse_is_false_when_null()
        {
            bool? statement = null;
            if (statement.IsFalse())
            {
                Assert.Fail();
            }
        }
    }
}
