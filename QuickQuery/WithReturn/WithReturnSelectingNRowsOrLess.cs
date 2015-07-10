namespace QckQuery
{
    using QckQuery.Formatting;
    using System.Data;

    public partial class QuickQuery
    {
        /// <summary>
        /// Runs the given query and returns the queried values.
        /// Throws UnexpectedNumberOfRowsSelected if the number of selected rows is greater than N.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="n">Number of selected rows to ensure</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>A DataTable with the queried values</returns>
        /// <exception cref="UnexpectedNumberOfRowsSelected">
        /// If the number of selected rows is greater than N
        /// </exception>
        public DataTable WithReturnSelectingNRowsOrLess(string sql, int n, object parameters)
        {
            return WithReturnSelectingNRows(sql, n, true, DictionaryMaker.Make(parameters));
        }

        /// <summary>
        /// Runs the given query and returns the queried values.
        /// Throws UnexpectedNumberOfRowsSelected if the number of selected rows is greater than N.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="n">Number of selected rows to ensure</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>A DataTable with the queried values</returns>
        /// <exception cref="UnexpectedNumberOfRowsSelected">
        /// If the number of selected rows is greater than N
        /// </exception>
        public DataTable WithReturnSelectingNRowsOrLess(string sql, int n, params object[] parameters)
        {
            return WithReturnSelectingNRows(sql, n, true, DictionaryMaker.Make(parameters));
        }
    }
}