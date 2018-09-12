namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;

    [TestClass]
    public class SelectTests : BaseTests
    {
        [TestMethod]
        public void DataTable()
        {
            var table = Query.Select(ConsoleSelect);

            for (var i = 0; i < table.Rows.Count; ++i)
            {
                var row = table.Rows[i];
                var expected = Console.NintendoConsoles[i];
                var actual = new Console
                {
                    Id = (long)row["Id"],
                    Name = (string)row["Name"],
                    HomeConsole = (bool)row["HomeConsole"],
                    PortableConsole = (bool)row["PortableConsole"],
                };

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void Empty() => Assert.IsFalse(
            Query.Select<Game>(GameSelect.Where("Console.Name = 'Dreamcast'")).Any()
        );

        [TestMethod]
        public void ExactlyN()
        {
            var expected = Console.NintendoConsoles.Where(c => c.PortableConsole).ToArray();
            var actual =
                Query.SelectExactly<Console>(expected.Length, ConsoleSelect.Where("PortableConsole = 1")).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsSelectedException))]
        public void UnexpectedExactlyN1() => Query.SelectExactly(1, "DELETE FROM Game");

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsSelectedException))]
        public void UnexpectedExactlyN2() => Query.SelectExactly(100, "DELETE FROM Game");

        [TestMethod]
        public void ExactlyOne() => Assert.AreEqual(
            Game.StarFox2,
            Query.SelectExactlyOne<Game>(GameSelect.Where("ReleaseDate IS NULL"))
        );

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsSelectedException))]
        public void UnexpectedExactlyOne1() => Query.SelectExactlyOne(GameSelect);

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsSelectedException))]
        public void UnexpectedExactlyOne2() => Query.SelectExactlyOne(GameSelect.Where("Game.Name = 'Star Fox 3'"));

        [TestMethod]
        public void JoinClause() => CollectionAssert.AreEqual(
            Game.StarFoxGames,
            Query.Select<Game>(GameSelect).ToArray()
        );

        [TestMethod]
        public void NoLessThanN() => Query.SelectNoLessThan(Game.StarFoxGames.Length, GameSelect);

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsSelectedException))]
        public void UnexpectedNoLessThanN() => Query.SelectNoLessThan(Game.StarFoxGames.Length + 1, GameSelect);

        [TestMethod]
        public void NoLessThanOne() => Query.SelectNoLessThanOne(GameSelect.Where("ReleaseDate IS NULL"));

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsSelectedException))]
        public void UnexpectedNoLessThanOne() =>
            Query.SelectNoLessThanOne(GameSelect.Where("Game.Name = 'Star Fox 3'"));

        [TestMethod]
        public void NoMoreThanN() => Query.SelectNoMoreThan(Game.StarFoxGames.Length, GameSelect);

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsSelectedException))]
        public void UnexpectedNoMoreThanN() => Query.SelectNoMoreThan(Game.StarFoxGames.Length - 1, GameSelect);

        [TestMethod]
        public void OrderBy() => CollectionAssert.AreEqual(
            Console.NintendoConsoles.OrderBy(c => c.Name).ToArray(),
            Query.Select<Console>(ConsoleSelect.OrderBy("NAME")).ToArray()
        );

        [TestMethod]
        public void OrderByDesc() => CollectionAssert.AreEqual(
            Console.NintendoConsoles.OrderByDescending(c => c.Name).ToArray(),
            Query.Select<Console>(ConsoleSelect.OrderByDesc("NAME")).ToArray()
        );

        [TestMethod]
        public void Single() => Assert.AreEqual(
            Game.StarFoxGames.Length,
            Query.SelectSingle<long>("SELECT COUNT(0) FROM Game")
        );

        [TestMethod]
        public void WhereAndMethod() => Assert.AreEqual(
            Console.Switch,
            Query.SelectExactlyOne<Console>(
                ConsoleSelect.WhereAnd("HomeConsole = 1").WhereAnd("PortableConsole = 1")
            )
        );
    }
}