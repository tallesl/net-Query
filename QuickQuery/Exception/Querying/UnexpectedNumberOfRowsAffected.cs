namespace QckQuery.Exception.Querying
{
    using System.Data.Common;
    using QckQuery.Formatting;

    /// <summary>
    /// Exception thrown when a query affects an unexpected number of rows.
    /// </summary>
    public class UnexpectedNumberOfRowsAffected : QuickQueryException
    {
        internal UnexpectedNumberOfRowsAffected(DbCommand command, int n) :
            base("The following query was rolled back because it affected " + n + " rows: " +
                command.GetFormattedSql() + ".") { }
    }
}
