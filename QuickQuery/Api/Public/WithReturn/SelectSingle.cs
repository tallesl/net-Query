namespace QckQuery
{
    using QckQuery.DataAccess;
    using QckQuery.Formatting;
    using System.Collections.Generic;

    public partial class QuickQuery
    {
        /// <summary>
        /// Runs the given query and returns the queried value.
        /// Only a single value (first column of the first row) is returned.
        /// It uses DbCommand.ExecuteScalar underneath.
        /// </summary>
        /// <typeparam name="T">Type of the value to be returned</typeparam>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>The first column of the first row queried</returns>
        public T SelectSingle<T>(string sql, object parameters)
        {
            return WithReturn<T>(sql, DictionaryMaker.Make(parameters));
        }
    }
}