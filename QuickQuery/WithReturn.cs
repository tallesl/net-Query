namespace QckQuery
{
    using System.Data;
    using QckQuery.DataAccess;
    using QckQuery.Exception.Querying;

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
        public T SingleValue<T>(string sql, params string[] parameters)
        {
            using (var connection = _connectionProvider.GetOpenConnection())
            using (var command = connection.GetCommand(sql, parameters))
            {
                return (T)command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Runs the given query and returns the queried values.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>A DataTable with the queried values</returns>
        public DataTable WithReturn(string sql, params string[] parameters)
        {
            using (var connection = _connectionProvider.GetOpenConnection())
            using (var command = connection.GetCommand(sql, parameters))
            {
                return _dataTableFiller.Fill(command);
            }
        }

        /// <summary>
        /// Runs the given query and returns the queried values.
        /// Throws UnexpectedNumberOfRowsSelected if none or more than one row was selected.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>A DataTable with the queried values</returns>
        /// <exception cref="UnexpectedNumberOfRowsAffected">If none or more than one row was selected</exception>
        public DataRow WithReturnSelectingExactlyOneRow(string sql, params string[] parameters)
        {
            var table = WithReturnSelectingNRows(sql, 1, false, parameters);
            return table.Rows[0];
        }

        /// <summary>
        /// Runs the given query and returns the queried values.
        /// Throws UnexpectedNumberOfRowsSelected if more than one row was selected.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>A DataTable with the queried values</returns>
        /// <exception cref="UnexpectedNumberOfRowsAffected">If more than one row was selected</exception>
        public DataRow WithReturnSelectingOneRowOrLess(string sql, params string[] parameters)
        {
            var table = WithReturnSelectingNRows(sql, 1, true, parameters);
            if (table.Rows.Count == 0) return null;
            else return table.Rows[0];
        }

        /// <summary>
        /// Runs the given query and returns the queried values.
        /// Throws UnexpectedNumberOfRowsSelected if the number of selected rows is different from N.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="n">Number of selected rows to ensure</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>A DataTable with the queried values</returns>
        /// <exception cref="UnexpectedNumberOfRowsAffected">
        /// If the number of selected rows is different from N
        /// </exception>
        public DataTable WithReturnSelectingExactlyNRows(string sql, int n, params string[] parameters)
        {
            return WithReturnSelectingNRows(sql, n, false, parameters);
        }

        /// <summary>
        /// Runs the given query and returns the queried values.
        /// Throws UnexpectedNumberOfRowsSelected if the number of selected rows is greater than N.
        /// It uses DbDataAdapter.Fill underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="n">Number of selected rows to ensure</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <returns>A DataTable with the queried values</returns>
        /// <exception cref="UnexpectedNumberOfRowsSelected">
        /// If the number of selected rows is greater than N
        /// </exception>
        public DataTable WithReturnSelectingNRowsOrLess(string sql, int n, params string[] parameters)
        {
            return WithReturnSelectingNRows(sql, n, true, parameters);
        }

        private DataTable WithReturnSelectingNRows(string sql, int n, bool acceptsLess, params string[] parameters)
        {
            using (var connection = _connectionProvider.GetOpenConnection())
            using (var command = connection.GetCommand(sql, parameters))
            {
                var dataTable = _dataTableFiller.Fill(command);
                var rows = dataTable.Rows.Count;
                if (rows > n || (!acceptsLess && rows != n)) return dataTable;
                else throw new UnexpectedNumberOfRowsSelected(command, rows);
            }
        }
    }
}
