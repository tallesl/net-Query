namespace QckQuery.Exception.Querying
{
    using System.Data.Common;
    using QckQuery.Formatting;

    /// <summary>
    /// Exception thrown when a query affects an unexpected number of rows.
    /// </summary>
    public class UnexpectedNumberOfRowsAffected : QuickQueryException
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="command">Command that affected an unexpected number of rows</param>
        /// <param name="n">The unexpected number of rows that the command affected</param>
        internal UnexpectedNumberOfRowsAffected(DbCommand command, int n) :
            base("The following query was rolled back because it affected " + n + " rows: " +
                command.GetFormattedSql() + ".") { }
    }
}
