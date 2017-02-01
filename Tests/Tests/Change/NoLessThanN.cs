namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public partial class ChangeTests : BaseTests
    {
        [TestMethod]
        public void NoLessThanN()
        {
            Query.ChangeNoLessThan(Game.StarFoxGames.Length, "DELETE FROM Game");
        }

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsAffectedException))]
        public void UnexpectedNoLessThanN()
        {
            Query.ChangeNoLessThan(Game.StarFoxGames.Length + 1, "DELETE FROM Game");
        }
    }
}
