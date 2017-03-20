namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public partial class ChangeTests : BaseTests
    {
        [TestMethod]
        public void ExactlyOne()
        {
            Query.ChangeExactlyOne("DELETE FROM Game WHERE ReleaseDate IS NULL");
        }

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsAffectedException))]
        public void UnexpectedExactlyOne1()
        {
            Query.ChangeExactlyOne("DELETE FROM Game");
        }

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsAffectedException))]
        public void UnexpectedExactlyOne2()
        {
            Query.ChangeExactlyOne("DELETE FROM Game WHERE Name = 'Star Fox 3'");
        }
    }
}
