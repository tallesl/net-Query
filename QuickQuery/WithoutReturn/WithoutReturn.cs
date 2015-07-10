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
        public void WithoutReturn(string sql, object parameters)
        {
            WithoutReturn(sql, DictionaryMaker.Make(parameters));
        }

        /// <summary>
        /// Runs the given query giving no return.
        /// It uses DbCommand.ExecuteNonQuery underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        public void WithoutReturn(string sql, params object[] parameters)
        {
            WithoutReturn(sql, DictionaryMaker.Make(parameters));
        }

        private void WithoutReturn(string sql, IDictionary<string, object> parameters)
        {
            using (var connection = _connectionProvider.Provide())
            using (var command = connection.GetCommandWithParametersSet(sql, parameters))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
