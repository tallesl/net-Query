namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public partial class ChangeTests : BaseTests
    {
        [TestMethod]
        public void NoMoreThanN()
        {
            Query.ChangeNoMoreThan(Game.StarFoxGames.Length, "DELETE FROM Game");
        }

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsAffectedException))]
        public void UnexpectedNoMoreThanN()
        {
            Query.ChangeNoMoreThan(Game.StarFoxGames.Length - 1, "DELETE FROM Game");
        }
    }
}
