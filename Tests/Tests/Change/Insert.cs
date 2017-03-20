namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;

    public partial class ChangeTests : BaseTests
    {
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
    }
}
