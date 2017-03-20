namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public partial class ChangeTests : BaseTests
    {
        [TestMethod]
        public void NoLessThanOne()
        {
            Query.ChangeNoLessThanOne("DELETE FROM Game WHERE ReleaseDate IS NULL");
        }

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsAffectedException))]
        public void UnexpectedNoLessThanOne()
        {
            Query.ChangeNoLessThanOne("DELETE FROM Game WHERE Name = 'Star Fox 3'");
        }
    }
}
