namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public partial class SelectTests : BaseTests
    {
        [TestMethod]
        public void ExactlyOne()
        {
            var starFox2 = Query.SelectExactlyOne<Game>(GameSelect.Where("ReleaseDate IS NULL"));

            Assert.AreEqual(Game.StarFox2, starFox2);
        }

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsSelectedException))]
        public void UnexpectedExactlyOne1()
        {
            Query.SelectExactlyOne(GameSelect);
        }

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsSelectedException))]
        public void UnexpectedExactlyOne2()
        {
            Query.SelectExactlyOne(GameSelect.Where("Game.Name = 'Star Fox 3'"));
        }
    }
}
