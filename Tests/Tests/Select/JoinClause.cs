namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;

    public partial class SelectTests : BaseTests
    {
        public void JoinClause()
        {
            var games = Query.Select<Game>(GameSelect);

            CollectionAssert.AreEqual(Game.StarFoxGames, games.ToArray());
        }
    }
}
