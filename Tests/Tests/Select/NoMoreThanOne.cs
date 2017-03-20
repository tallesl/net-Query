namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public partial class SelectTests : BaseTests
    {
        [TestMethod]
        public void NoMoreThanOne()
        {
            Query.SelectNoMoreThanOne(GameSelect.Where("ReleaseDate IS NULL"));
        }

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsSelectedException))]
        public void UnexpectedNoMoreThanOne()
        {
            Query.SelectNoMoreThanOne(GameSelect);
        }
    }
}
