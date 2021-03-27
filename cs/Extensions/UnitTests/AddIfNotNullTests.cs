using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using UncategorizedExtensions;

namespace UnitTests
{
    [TestClass]
    public class AddIfNotNullTests
    {
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
    }
}
