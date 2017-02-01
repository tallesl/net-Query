namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public partial class SelectTests : BaseTests
    {
        [TestMethod]
        public void NoLessThanN()
        {
            Query.SelectNoLessThan(Game.StarFoxGames.Length, GameSelect);
        }

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsSelectedException))]
        public void UnexpectedNoLessThanN()
        {
            Query.SelectNoLessThan(Game.StarFoxGames.Length + 1, GameSelect);
        }
    }
}
