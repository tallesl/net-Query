namespace QckQuery
{
    using QckQuery.DataAccess;
    using QckQuery.Formatting;
    using System.Collections.Generic;
    using System.Data;

    public partial class QuickQuery
    {
        /// <summary>
        /// Runs the given query and returns the queried values.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>A DataTable with the queried values</returns>
        public DataTable WithReturn(string sql, object parameters)
        {
            return WithReturn(sql, DictionaryMaker.Make(parameters));
        }

        /// <summary>
        /// Runs the given query and returns the queried values.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>A DataTable with the queried values</returns>
        public DataTable WithReturn(string sql, params object[] parameters)
        {
            return WithReturn(sql, DictionaryMaker.Make(parameters));
        }

        private DataTable WithReturn(string sql, IDictionary<string, object> parameters)
        {
            using (var connection = _connectionProvider.Provide())
            using (var command = connection.GetCommandWithParametersSet(sql, parameters))
            {
                return _dataTableFiller.Fill(command);
            }
        }
    }
}