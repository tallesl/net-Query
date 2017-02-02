namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Data.SQLite;
    using System.Linq;

    public partial class ChangeTests : BaseTests
    {
        [TestMethod]
        public void ManualRollback()
        {
            using (var query = ManualQuery)
            {
                query.Change("DELETE FROM Game");

                try { query.Change("some syntax error"); }
                catch (SQLiteException) { }
            }

            var expected = Game.StarFoxGames;
            var actual = Query.Select<Game>(GameSelect).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AutomaticRollback()
        {
            try
            {
                Query.ChangeExactlyOne("DELETE FROM Game");
            }
            catch (UnexpectedNumberOfRowsAffectedException) { }

            var expected = Game.StarFoxGames;
            var actual = Query.Select<Game>(GameSelect).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
