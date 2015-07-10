namespace QckQuery
{
    using QckQuery.Formatting;
    using System.Data;
    using System.Linq;

    public partial class QuickQuery
    {
        /// <summary>
        /// Runs the given query and returns the queried values.
        /// Throws UnexpectedNumberOfRowsSelected if none or more than one row was selected.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>A DataTable with the queried values</returns>
        /// <exception cref="UnexpectedNumberOfRowsAffected">If none or more than one row was selected</exception>
        public DataRow WithReturnSelectingExactlyOneRow(string sql, object parameters)
        {
            return WithReturnSelectingNRows(sql, 1, false, DictionaryMaker.Make(parameters)).AsEnumerable().First();
        }

        /// <summary>
        /// Runs the given query and returns the queried values.
        /// Throws UnexpectedNumberOfRowsSelected if none or more than one row was selected.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>A DataTable with the queried values</returns>
        /// <exception cref="UnexpectedNumberOfRowsAffected">If none or more than one row was selected</exception>
        public DataRow WithReturnSelectingExactlyOneRow(string sql, params object[] parameters)
        {
            return WithReturnSelectingNRows(sql, 1, false, DictionaryMaker.Make(parameters)).AsEnumerable().First();
        }
    }
}