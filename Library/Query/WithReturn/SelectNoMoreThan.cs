namespace QueryLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using ObjectLibrary;

    public sealed partial class Query
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
        /// <exception cref="UnexpectedNumberOfRowsSelectedException">
        /// If the number of selected rows is greater than N
        /// </exception>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "n")]
        public DataTable SelectNoMoreThan(int n, string sql, object parameters = null)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            return WithReturn(sql, parameters, CountValidationEnum.NoMoreThan, n);
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
        /// <exception cref="UnexpectedNumberOfRowsSelectedException">
        /// If the number of selected rows is greater than N
        /// </exception>
        /// <exception cref="MismatchedTypesException">
        /// The corresponding type in the given class is different than the one found in the DataTable
        /// </exception>
        /// <exception cref="PropertyNotFoundException">
        /// A column of the DataTable doesn't match any in the given class
        /// </exception>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "n")]
        public IEnumerable<T> SelectNoMoreThan<T>(int n, string sql, object parameters = null) where T : new()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            return _options.Safe ?
                SelectNoMoreThan(n, sql, parameters).ToObjectSafe<T>() :
                SelectNoMoreThan(n, sql, parameters).ToObject<T>();
        }
    }
}