namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public partial class ChangeTests : BaseTests
    {
        [TestMethod]
        public void Update()
        {
            Query.ChangeExactlyOne("UPDATE Game SET Name = 'Starwing' WHERE Name = 'Star Fox'");

            var actual = Query.SelectSingle<string>("SELECT Name FROM Game WHERE Id = 1");

            Assert.AreEqual("Starwing", actual);
        }
    }
}
