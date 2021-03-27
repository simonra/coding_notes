using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Uncategorized;

namespace UnitTests
{
    [TestClass]
    public class UncategorizedTests
    {
        [TestMethod]
        public void StrJoin_joins_list()
        {
            var list = new List<string> { "a", "b", "c" };

            var joinedList = list.StrJoin(",");

            Assert.AreEqual("a,b,c", joinedList);
        }

        [TestMethod]
        public void AddIfNotNull_adds_if_item_not_null()
        {
            var collection = new List<string>();
            var item = "hello unit test";

            collection.AddIfNotNull(item);

            Assert.AreEqual(item, collection[0]);
        }

        [TestMethod]
        public void AddIfNotNull_doesnt_add_if_item_is_null()
        {
            var collection = new List<string>();
            string? item = null;

            collection.AddIfNotNull(item);

            Assert.AreEqual(0, collection.Count, "Null should not have been added.");
        }

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
