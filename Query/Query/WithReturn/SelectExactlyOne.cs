namespace QueryLibrary
{
    using System;
    using System.Data;
    using System.Linq;
    using ObjectLibrary;

    public sealed partial class Query
    {
        /// <summary>
        /// Runs the given query and returns the queried values.
        /// Throws UnexpectedNumberOfRowsSelected if more than one or none row is selected.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>A DataTable with the queried values</returns>
        /// <exception cref="UnexpectedNumberOfRowsAffectedException">
        /// If more than one or none row is selected
        /// </exception>
        public DataTable SelectExactlyOne(string sql, object parameters = null)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

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
        /// <exception cref="UnexpectedNumberOfRowsAffectedException">
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
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            return SelectExactly<T>(1, sql, parameters).Single();
        }
    }
}