namespace QckQuery
{
    using QckQuery.Exceptions;
    using System.Data;
    using System.Linq;
    using ToObject.Exceptions;

    public partial class QuickQuery
    {
        /// <summary>
        /// Runs the given query and returns the queried values.
        /// Throws UnexpectedNumberOfRowsSelected if more than one row is selected.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>A DataTable with the queried values</returns>
        /// <exception cref="UnexpectedNumberOfRowsSelected">
        /// If more than one row is selected
        /// </exception>
        public DataTable SelectNoMoreThanOne(string sql, object parameters = null)
        {
            return SelectNoMoreThan(1, sql, parameters);
        }

        /// <summary>
        /// Runs the given query and returns the queried values.
        /// Throws UnexpectedNumberOfRowsSelected if more than one row is selected.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>The querie objects</returns>
        /// <exception cref="UnexpectedNumberOfRowsSelected">
        /// If more than one row is selected
        /// </exception>
        /// <exception cref="MismatchedTypesException">
        /// The corresponding type in the given class is different than the one found in the DataTable
        /// </exception>
        /// <exception cref="PropertyNotFoundException">
        /// A column of the DataTable doesn't match any in the given class
        /// </exception>
        public T SelectNoMoreThanOne<T>(string sql, object parameters = null) where T : new()
        {
            return SelectNoMoreThan<T>(1, sql, parameters).SingleOrDefault();
        }
    }
}