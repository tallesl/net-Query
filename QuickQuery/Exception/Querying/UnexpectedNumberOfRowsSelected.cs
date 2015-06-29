namespace QckQuery.Exception.Querying
{
    using System.Data.Common;
    using QckQuery.Formatting;

    /// <summary>
    /// Exception thrown when a query selects an unexpected number of rows.
    /// </summary>
    public class UnexpectedNumberOfRowsSelected : QuickQueryException
    {
        internal UnexpectedNumberOfRowsSelected(DbCommand command, int n) :
            base("The following query selected an unexpected number of rows (" + n + "): " +
                command.GetFormattedSql() + ".") { }
    }
}
