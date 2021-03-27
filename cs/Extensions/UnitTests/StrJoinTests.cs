using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using UncategorizedExtensions;

namespace UnitTests
{
    [TestClass]
    public class StrJoinTests
    {
        [TestMethod]
        public void StrJoin_joins_list()
        {
            var list = new List<string> { "a", "b", "c" };

            var joinedList = list.StrJoin(",");

            Assert.AreEqual("a,b,c", joinedList);
        }
    }
}
