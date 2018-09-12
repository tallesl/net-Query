namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;
    using System.Data.Common;
    using System.Data.SQLite;

    [TestClass]
    public class BaseTests
    {
        protected Query Query
        {
            get => new Query("Data Source=Test.db", "System.Data.SQLite");
        }

        protected Query ManualQuery
        {
            get => new Query("Data Source=Test.db", "System.Data.SQLite", new QueryOptions { ManualClosing = true });
        }

        protected Select ConsoleSelect
        {
            get => new Select("*").From("Console");
        }

        protected Select GameSelect
        {
            get => new Select(
                "Game.Id",
                "Game.Name",
                "Console.Id AS 'Console.Id'",
                "Console.Name AS 'Console.Name'",
                "Console.HomeConsole AS 'Console.HomeConsole'",
                "Console.PortableConsole AS 'Console.PortableConsole'",
                "Game.ReleaseDate"
            )
            .From("Game")
            .Join("Console", "Console.Id = Game.ConsoleId");
        }

        [TestInitialize]
        public void Initialize()
        {
            DbProviderFactories.RegisterFactory(
                "System.Data.SQLite", "System.Data.SQLite.SQLiteFactory, System.Data.SQLite");

            File.Delete("Test.db");

            using (var query = ManualQuery)
            {
                query.Change(@"
                    CREATE TABLE Console (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        HomeConsole BOOLEAN NOT NULL,
                        PortableConsole BOOLEAN NOT NULL
                    )
                ");

                query.Change(@"
                    CREATE TABLE Game (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        ConsoleId INTEGER NOT NULL,
                        ReleaseDate DATETIME NULL
                    )
                ");

                foreach (var console in Console.NintendoConsoles)
                {
                    query.Change(
                        "INSERT INTO Console (Name, HomeConsole, PortableConsole) " +
                        "VALUES (@Name, @HomeConsole, @PortableConsole)",
                        new
                        {
                            console.Name,
                            console.HomeConsole,
                            console.PortableConsole,
                        }
                    );

                    console.Id = query.SelectSingle<long>("SELECT LAST_INSERT_ROWID()");
                }

                foreach (var game in Game.StarFoxGames)
                {
                    query.Change(
                        "INSERT INTO Game (Name, ConsoleId, ReleaseDate) VALUES (@Name, @ConsoleId, @ReleaseDate)",
                        new
                        {
                            game.Name,
                            ConsoleId = game.Console.Id,
                            game.ReleaseDate,
                        }
                    );

                    game.Id = query.SelectSingle<long>("SELECT LAST_INSERT_ROWID()");
                }
            }
        }
    }
}
