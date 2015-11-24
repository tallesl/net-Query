namespace QckQuery
{
    using QckQuery.Exceptions;
    using System.Collections.Generic;
    using System.Data;
    using ToObject;
    using ToObject.Exceptions;

    public partial class QuickQuery
    {
        /// <summary>
        /// Runs the given query and returns the queried values.
        /// Throws UnexpectedNumberOfRowsSelected if the number of selected rows is greater than N.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="n">Number of selected rows to ensure</param>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>A DataTable with the queried values</returns>
        /// <exception cref="UnexpectedNumberOfRowsSelected">
        /// If the number of selected rows is greater than N
        /// </exception>
        public DataTable SelectNoMoreThan(int n, string sql, object parameters = null)
        {
            return WithReturn(sql, parameters, n, true);
        }

        /// <summary>
        /// Runs the given query and returns the queried values.
        /// Throws UnexpectedNumberOfRowsSelected if the number of selected rows is greater than N.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="n">Number of selected rows to ensure</param>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>The queried objects</returns>
        /// <exception cref="UnexpectedNumberOfRowsSelected">
        /// If the number of selected rows is greater than N
        /// </exception>
        /// <exception cref="MismatchedTypesException">
        /// The corresponding type in the given class is different than the one found in the DataTable
        /// </exception>
        /// <exception cref="PropertyNotFoundException">
        /// A column of the DataTable doesn't match any in the given class
        /// </exception>
        public IEnumerable<T> SelectNoMoreThan<T>(int n, string sql, object parameters = null) where T : new()
        {
            return _configuration.Safe ?
                SelectNoMoreThan(n, sql, parameters).ToObjectSafe<T>() :
                SelectNoMoreThan(n, sql, parameters).ToObject<T>();
        }
    }
}