namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;

    [TestClass]
    public partial class ChangeTests : BaseTests
    {
        [TestMethod]
        public void Delete()
        {
            Query.Change("DELETE FROM Game WHERE Id = 4");

            var actual = Query.Select<Game>(GameSelect).ToArray();
            var expected = Game.StarFoxGames.Where(g => g.Id != 4).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
