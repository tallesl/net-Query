namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;

    [TestClass]
    public partial class ChangeTests : BaseTests
    {
        [TestMethod]
        public void Delete()
        {
            Query.Change("DELETE FROM Game WHERE Id = 4");

            var actual = Query.Select<Game>(GameSelect).ToArray();
            var expected = Game.StarFoxGames.Where(g => g.Id != 4).ToArray();

            CollectionAssert.AreEqual(expected, actual);
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
    }
}
