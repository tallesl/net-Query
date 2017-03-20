namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public partial class SelectTests : BaseTests
    {
        [TestMethod]
        public void WhereAndMethod()
        {
            var _switch = Query.SelectExactlyOne<Console>(
                ConsoleSelect.WhereAnd("HomeConsole = 1").WhereAnd("PortableConsole = 1"));

            Assert.AreEqual(Console.Switch, _switch);
        }
    }
}
