namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;

    public partial class SelectTests : BaseTests
    {
        [TestMethod]
        public void OrderBy()
        {
            var expected = Console.NintendoConsoles.OrderBy(c => c.Name).ToArray();
            var actual = Query.Select<Console>(ConsoleSelect.OrderBy("NAME")).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void OrderByDesc()
        {
            var expected = Console.NintendoConsoles.OrderByDescending(c => c.Name).ToArray();
            var actual = Query.Select<Console>(ConsoleSelect.OrderByDesc("NAME")).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
