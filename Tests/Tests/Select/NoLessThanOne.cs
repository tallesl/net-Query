namespace QueryLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public partial class SelectTests : BaseTests
    {
        [TestMethod]
        public void NoLessThanOne()
        {
            Query.SelectNoLessThanOne(GameSelect.Where("ReleaseDate IS NULL"));
        }

        [TestMethod, ExpectedException(typeof(UnexpectedNumberOfRowsSelectedException))]
        public void UnexpectedNoLessThanOne()
        {
            Query.SelectNoLessThanOne(GameSelect.Where("Game.Name = 'Star Fox 3'"));
        }
    }
}
