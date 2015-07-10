namespace QckQuery.Exceptions.Querying
{
    using QckQuery.Formatting;
    using System.Data.Common;

    /// <summary>
    /// Exception thrown when a query selects an unexpected number of rows.
    /// </summary>
    public class UnexpectedNumberOfRowsSelected : QuickQueryException
    {
        internal UnexpectedNumberOfRowsSelected(DbCommand command, int n)
            : base(string.Format("The following query selected an unexpected number of rows ({0}): {1}.",
                n, SqlFormatter.Format(command))) { }
    }
}
