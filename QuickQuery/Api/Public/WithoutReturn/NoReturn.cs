namespace QckQuery
{
    using QckQuery.DataAccess;
    using QckQuery.Formatting;
    using System.Collections.Generic;

    public partial class QuickQuery
    {
        /// <summary>
        /// Runs the given query giving no return.
        /// It uses DbCommand.ExecuteNonQuery underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        public void NoReturn(string sql, object parameters)
        {
            WithoutReturn(sql, DictionaryMaker.Make(parameters));
        }

        /// <summary>
        /// Runs the given query giving no return.
        /// It uses DbCommand.ExecuteNonQuery underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        public void NoReturn(string sql, params object[] parameters)
        {
            WithoutReturn(sql, DictionaryMaker.Make(parameters));
        }
    }
}
