namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public partial class ChangeTests : BaseTests
    {
        [TestMethod]
        public void NoMoreThanOne()
        {
            Query.ChangeNoMoreThanOne("DELETE FROM Game WHERE ReleaseDate IS NULL");
        }

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsAffectedException))]
        public void UnexpectedNoMoreThanOne()
        {
            Query.ChangeNoMoreThanOne("DELETE FROM Game");
        }
    }
}
