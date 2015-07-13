namespace QckQuery
{
    using DataTableToObject;
    using DataTableToObject.Exceptions;
    using QckQuery.Exceptions.Querying;
    using QckQuery.Formatting;
    using System.Data;
    using System.Linq;

    public partial class QuickQuery
    {
        /// <summary>
        /// Runs the given query and returns the queried values.
        /// Throws UnexpectedNumberOfRowsSelected if more than one row was selected.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>A DataTable with the queried values</returns>
        public DataRow WithReturnSelectingOneRowOrLess(string sql, object parameters)
        {
            return WithReturnSelectingNRows(sql, 1, true, DictionaryMaker.Make(parameters))
                .AsEnumerable().FirstOrDefault();
        }

        /// <summary>
        /// Runs the given query and returns the queried values.
        /// Throws UnexpectedNumberOfRowsSelected if more than one row was selected.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>A DataTable with the queried values</returns>
        /// <exception cref="UnexpectedNumberOfRowsAffected">If more than one row was selected</exception>
        /// <exception cref="MismatchedTypesException">
        /// The corresponding type in the given class is different than the one found in the DataRow
        /// </exception>
        /// <exception cref="PropertyNotFoundException">
        /// A column of the DataRow doesn't match any in the given class
        /// </exception>
        public T WithReturnSelectingOneRowOrLess<T>(string sql, object parameters) where T : new()
        {
            var row = WithReturnSelectingNRows(sql, 1, true, DictionaryMaker.Make(parameters))
                .AsEnumerable().FirstOrDefault();
            return row == null ? default(T) : row.ToObject<T>();
        }

        /// <summary>
        /// Runs the given query and returns the queried values.
        /// Throws UnexpectedNumberOfRowsSelected if more than one row was selected.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>A DataTable with the queried values</returns>
        /// <exception cref="UnexpectedNumberOfRowsAffected">If more than one row was selected</exception>
        public DataRow WithReturnSelectingOneRowOrLess(string sql, params object[] parameters)
        {
            return WithReturnSelectingNRows(sql, 1, true, DictionaryMaker.Make(parameters))
                .AsEnumerable().FirstOrDefault();
        }

        /// <summary>
        /// Runs the given query and returns the queried values.
        /// Throws UnexpectedNumberOfRowsSelected if more than one row was selected.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>A DataTable with the queried values</returns>
        /// <exception cref="UnexpectedNumberOfRowsAffected">If more than one row was selected</exception>
        /// <exception cref="MismatchedTypesException">
        /// The corresponding type in the given class is different than the one found in the DataRow
        /// </exception>
        /// <exception cref="PropertyNotFoundException">
        /// A column of the DataRow doesn't match any in the given class
        /// </exception>
        public T WithReturnSelectingOneRowOrLess<T>(string sql, params object[] parameters) where T : new()
        {
            var row = WithReturnSelectingNRows(sql, 1, true, DictionaryMaker.Make(parameters))
                .AsEnumerable().FirstOrDefault();
            return row == null ? default(T) : row.ToObject<T>();
        }
    }
}