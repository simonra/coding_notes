﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using UncategorizedExtensions;

namespace UnitTests
{
    [TestClass]
    public class WhereNotNullClassTests
    {
        [TestMethod]
        public void WhereNotNull_removes_items_that_are_null()
        {
            var listWithNull =
                new List<ReferenceType?>()
                {
                    new ReferenceType { Field = string.Empty },
                    new ReferenceType(),
                    null,
                    new ReferenceType { Field = null },
                    new ReferenceType { Field = "4" },
                    null
                };

            var listWithoutNull = listWithNull.WhereNotNull();

            Assert.AreEqual(4, listWithoutNull.Count());
        }

        [TestMethod]
        public void WhereNotNull_output_usable_in_interface_that_doesnt_accept_null()
        {
            var listWithNull =
                new List<ReferenceType?>()
                {
                    new ReferenceType { Field = string.Empty },
                    new ReferenceType(),
                    null,
                    new ReferenceType { Field = null },
                    new ReferenceType { Field = "4" },
                    null
                };

            var listWithoutNull = listWithNull.WhereNotNull();

            OnlyAcceptsNonNullableItems(listWithoutNull);
        }

        private static void OnlyAcceptsNonNullableItems(IEnumerable<ReferenceType> items)
        {
            foreach(var item in items)
            {
                Assert.IsNotNull(item);
            }
        }

        private class ReferenceType
        {
            public string? Field { get; set; }
        }
    }
}
