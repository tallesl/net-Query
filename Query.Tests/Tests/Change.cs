namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Data.SQLite;
    using System.Linq;
    using System;

    [TestClass]
    public class ChangeTests : BaseTests
    {
        [TestMethod]
        public void Delete()
        {
            Query.Change("DELETE FROM Game WHERE Id = 4");

            CollectionAssert.AreEqual(
                Game.StarFoxGames.Where(g => g.Id != 4).ToArray(),
                Query.Select<Game>(GameSelect).ToArray()
            );
        }

        [TestMethod]
        public void ManualDelete()
        {
            Func<Game[]> actualGames = () => Query.Select<Game>(GameSelect).ToArray();
            Func<Console[]> actualConsoles = () => Query.Select<Console>(ConsoleSelect).ToArray();

            var query = ManualQuery;

            query.Change("DELETE FROM Game WHERE ConsoleId IN (SELECT Id FROM Console WHERE PortableConsole = 1)");
            query.Change("DELETE FROM Console WHERE PortableConsole = 1");

            CollectionAssert.AreEqual(Game.StarFoxGames.ToArray(), actualGames());
            CollectionAssert.AreEqual(Console.NintendoConsoles.ToArray(), actualConsoles());

            query.Close();

            CollectionAssert.AreEqual(
                Game.StarFoxGames.Where(g => !g.Console.PortableConsole).ToArray(),
                actualGames()
            );

            CollectionAssert.AreEqual(
                Console.NintendoConsoles.Where(c => !c.PortableConsole).ToArray(),
                actualConsoles()
            );
        }

        [TestMethod]
        public void ExactlyN() => Query.ChangeExactly(Game.StarFoxGames.Length, "DELETE FROM Game");

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsAffectedException))]
        public void UnexpectedExactlyN1() => Query.ChangeExactly(1, "DELETE FROM Game");

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsAffectedException))]
        public void UnexpectedExactlyN2() => Query.ChangeExactly(100, "DELETE FROM Game");

        [TestMethod]
        public void ExactlyOne() => Query.ChangeExactlyOne("DELETE FROM Game WHERE ReleaseDate IS NULL");

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsAffectedException))]
        public void UnexpectedExactlyOne1() => Query.ChangeExactlyOne("DELETE FROM Game");

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsAffectedException))]
        public void UnexpectedExactlyOne2() => Query.ChangeExactlyOne("DELETE FROM Game WHERE Name = 'Star Fox 3'");

        [TestMethod]
        public void Insert()
        {
            Query.Change(
                @"
                    INSERT INTO Console (Name, HomeConsole, PortableConsole)
                    VALUES (@Name, @HomeConsole, @PortableConsole)
                ",
                new
                {
                    Name = "Virtual Boy",
                    HomeConsole = false,
                    PortableConsole = true,
                }
            );

            var virtualBoy = Query.SelectExactlyOne<Console>(ConsoleSelect.Where("Id = 13"));
            var expected = new Console[Console.NintendoConsoles.Length + 1];
            var actual = Query.Select<Console>(ConsoleSelect).ToArray();

            Array.Copy(Console.NintendoConsoles, expected, Console.NintendoConsoles.Length);
            expected[expected.Length - 1] = virtualBoy;

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NoLessThanN() => Query.ChangeNoLessThan(Game.StarFoxGames.Length, "DELETE FROM Game");

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsAffectedException))]
        public void UnexpectedNoLessThanN() => Query.ChangeNoLessThan(Game.StarFoxGames.Length + 1, "DELETE FROM Game");

        [TestMethod]
        public void NoLessThanOne() => Query.ChangeNoLessThanOne("DELETE FROM Game WHERE ReleaseDate IS NULL");

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsAffectedException))]
        public void UnexpectedNoLessThanOne() => Query.ChangeNoLessThanOne("DELETE FROM Game WHERE Name = 'Star Fox 3'");

        [TestMethod]
        public void NoMoreThanN() => Query.ChangeNoMoreThan(Game.StarFoxGames.Length, "DELETE FROM Game");

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsAffectedException))]
        public void UnexpectedNoMoreThanN() => Query.ChangeNoMoreThan(Game.StarFoxGames.Length - 1, "DELETE FROM Game");

        [TestMethod]
        public void NoMoreThanOne() => Query.ChangeNoMoreThanOne("DELETE FROM Game WHERE ReleaseDate IS NULL");

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsAffectedException))]
        public void UnexpectedNoMoreThanOne() => Query.ChangeNoMoreThanOne("DELETE FROM Game");

        [TestMethod]
        public void ManualRollback()
        {
            using (var query = ManualQuery)
            {
                query.Change("DELETE FROM Game");

                try { query.Change("some syntax error"); }
                catch (SQLiteException) { }
            }

            CollectionAssert.AreEqual(
                Game.StarFoxGames,
                Query.Select<Game>(GameSelect).ToArray()
            );
        }

        [TestMethod]
        public void AutomaticRollback()
        {
            try
            {
                Query.ChangeExactlyOne("DELETE FROM Game");
            }
            catch (UnexpectedNumberOfRowsAffectedException) { }

            CollectionAssert.AreEqual(
                Game.StarFoxGames,
                Query.Select<Game>(GameSelect).ToArray()
            );
        }

        [TestMethod]
        public void Update()
        {
            Query.ChangeExactlyOne("UPDATE Game SET Name = 'Starwing' WHERE Name = 'Star Fox'");

            Assert.AreEqual(
                "Starwing",
                Query.SelectSingle<string>("SELECT Name FROM Game WHERE Id = 1")
            );
        }
    }
}