namespace QckQuery.Exception.Querying
{
    using System.Data.Common;
    using QckQuery.Formatting;

    /// <summary>
    /// Exception thrown when a query selects an unexpected number of rows.
    /// </summary>
    public class UnexpectedNumberOfRowsSelected : QuickQueryException
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="command">Command that selected an unexpected number of rows</param>
        /// <param name="n">The unexpected number of rows that the command selected</param>
        internal UnexpectedNumberOfRowsSelected(DbCommand command, int n) :
            base("The following query selected an unexpected number of rows (" + n + "): " +
                command.GetFormattedSql() + ".") { }
    }
}
