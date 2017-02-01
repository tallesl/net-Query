namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public partial class SelectTests : BaseTests
    {
        [TestMethod]
        public void NoMoreThanN()
        {
            Query.SelectNoMoreThan(Game.StarFoxGames.Length, GameSelect);
        }

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsSelectedException))]
        public void UnexpectedNoMoreThanN()
        {
            Query.SelectNoMoreThan(Game.StarFoxGames.Length - 1, GameSelect);
        }
    }
}
