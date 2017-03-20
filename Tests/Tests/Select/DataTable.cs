namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public partial class SelectTests : BaseTests
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
    }
}
