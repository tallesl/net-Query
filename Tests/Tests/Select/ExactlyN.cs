namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;

    public partial class SelectTests : BaseTests
    {
        [TestMethod]
        public void ExactlyN()
        {
            var expected = Console.NintendoConsoles.Where(c => c.PortableConsole).ToArray();
            var actual =
                Query.SelectExactly<Console>(expected.Length, ConsoleSelect.Where("PortableConsole = 1")).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsSelectedException))]
        public void UnexpectedExactlyN1()
        {
            Query.SelectExactly(1, "DELETE FROM Game");
        }

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsSelectedException))]
        public void UnexpectedExactlyN2()
        {
            Query.SelectExactly(100, "DELETE FROM Game");
        }
    }
}
