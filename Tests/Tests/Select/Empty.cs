namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;

    public partial class SelectTests : BaseTests
    {
        [TestMethod]
        public void Empty()
        {
            var empty = Query.Select<Game>(GameSelect.Where("Console.Name = 'Dreamcast'"));

            Assert.IsFalse(empty.Any());
        }
    }
}
