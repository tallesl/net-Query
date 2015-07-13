namespace QckQuery
{
    using DataTableToObject;
    using DataTableToObject.Exceptions;
    using QckQuery.Exceptions.Querying;
    using QckQuery.Formatting;
    using System.Collections.Generic;
    using System.Data;

    public partial class QuickQuery
    {
        /// <summary>
        /// Runs the given query and returns the queried values.
        /// Throws UnexpectedNumberOfRowsSelected if the number of selected rows is different from N.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="n">Number of selected rows to ensure</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>A DataTable with the queried values</returns>
        /// <exception cref="UnexpectedNumberOfRowsAffected">
        /// If the number of selected rows is different from N
        /// </exception>
        public DataTable WithReturnSelectingExactlyNRows(string sql, int n, object parameters)
        {
            return WithReturnSelectingNRows(sql, n, false, DictionaryMaker.Make(parameters));
        }

        /// <summary>
        /// Runs the given query and returns the queried values.
        /// Throws UnexpectedNumberOfRowsSelected if the number of selected rows is different from N.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="n">Number of selected rows to ensure</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>A DataTable with the queried values</returns>
        /// <exception cref="UnexpectedNumberOfRowsAffected">
        /// If the number of selected rows is different from N
        /// </exception>
        /// <exception cref="MismatchedTypesException">
        /// The corresponding type in the given class is different than the one found in the DataTable
        /// </exception>
        /// <exception cref="PropertyNotFoundException">
        /// A column of the DataTable doesn't match any in the given class
        /// </exception>
        public IEnumerable<T> WithReturnSelectingExactlyNRows<T>(string sql, int n, object parameters) where T : new()
        {
            return WithReturnSelectingNRows(sql, n, false, DictionaryMaker.Make(parameters)).ToObject<T>();
        }

        /// <summary>
        /// Runs the given query and returns the queried values.
        /// Throws UnexpectedNumberOfRowsSelected if the number of selected rows is different from N.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="n">Number of selected rows to ensure</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>A DataTable with the queried values</returns>
        /// <exception cref="UnexpectedNumberOfRowsAffected">
        /// If the number of selected rows is different from N
        /// </exception>
        public DataTable WithReturnSelectingExactlyNRows(string sql, int n, params object[] parameters)
        {
            return WithReturnSelectingNRows(sql, n, false, DictionaryMaker.Make(parameters));
        }

        /// <summary>
        /// Runs the given query and returns the queried values.
        /// Throws UnexpectedNumberOfRowsSelected if the number of selected rows is different from N.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="n">Number of selected rows to ensure</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>A DataTable with the queried values</returns>
        /// <exception cref="UnexpectedNumberOfRowsAffected">
        /// If the number of selected rows is different from N
        /// </exception>
        /// <exception cref="MismatchedTypesException">
        /// The corresponding type in the given class is different than the one found in the DataTable
        /// </exception>
        /// <exception cref="PropertyNotFoundException">
        /// A column of the DataTable doesn't match any in the given class
        /// </exception>
        public IEnumerable<T> WithReturnSelectingExactlyNRows<T>(string sql, int n, params object[] parameters)
            where T : new()
        {
            return WithReturnSelectingNRows(sql, n, false, DictionaryMaker.Make(parameters)).ToObject<T>();
        }
    }
}