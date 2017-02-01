namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public partial class ChangeTests : BaseTests
    {
        [TestMethod]
        public void ExactlyN()
        {
            Query.ChangeExactly(Game.StarFoxGames.Length, "DELETE FROM Game");
        }

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsAffectedException))]
        public void UnexpectedExactlyN1()
        {
            Query.ChangeExactly(1, "DELETE FROM Game");
        }

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsAffectedException))]
        public void UnexpectedExactlyN2()
        {
            Query.ChangeExactly(100, "DELETE FROM Game");
        }
    }
}
