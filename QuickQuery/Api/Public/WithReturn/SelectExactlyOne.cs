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
        /// Throws UnexpectedNumberOfRowsSelected if more than one or none row is selected.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>A DataTable with the queried values</returns>
        /// <exception cref="UnexpectedNumberOfRowsAffected">
        /// If more than one or none row is selected
        /// </exception>
        public DataTable SelectExactlyOne(string sql, object parameters = null)
        {
            return SelectExactly(1, sql, parameters);
        }

        /// <summary>
        /// Runs the given query and returns the queried values.
        /// Throws UnexpectedNumberOfRowsSelected if more than one or none row is selected.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>The queried object</returns>
        /// <exception cref="UnexpectedNumberOfRowsAffected">
        /// If more than one or none row is selected
        /// </exception>
        /// <exception cref="MismatchedTypesException">
        /// The corresponding type in the given class is different than the one found in the DataTable
        /// </exception>
        /// <exception cref="PropertyNotFoundException">
        /// A column of the DataTable doesn't match any in the given class
        /// </exception>
        public T SelectExactlyOne<T>(string sql, object parameters = null) where T : new()
        {
            return SelectExactly<T>(1, sql, parameters).Single();
        }
    }
}