namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public partial class SelectTests : BaseTests
    {
        [TestMethod]
        public void Single()
        {
            var games = Query.SelectSingle<long>("SELECT COUNT(0) FROM Game");

            Assert.AreEqual(Game.StarFoxGames.Length, games);
        }
    }
}
