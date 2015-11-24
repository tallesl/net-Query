namespace QueryLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using ToObject;
    using ToObject.Exceptions;

    public partial class Query
    {
        /// <summary>
        /// Runs the given query and returns the queried values.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>A DataTable with the queried values</returns>
        public DataTable Select(string sql, object parameters = null)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            return WithReturn(sql, parameters);
        }

        /// <summary>
        /// Runs the given query and returns the queried values.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>The queried objects</returns>
        /// <exception cref="MismatchedTypesException">
        /// The corresponding type in the given class is different than the one found in the DataTable
        /// </exception>
        /// <exception cref="PropertyNotFoundException">
        /// A column of the DataTable doesn't match any in the given class
        /// </exception>
        public IEnumerable<T> Select<T>(string sql, object parameters = null) where T : new()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            return _options.Safe ?
                Select(sql, parameters).ToObjectSafe<T>() :
                Select(sql, parameters).ToObject<T>();
        }
    }
}