namespace QckQuery
{
    using QckQuery.DataAccess;
    using QckQuery.Exceptions.Querying;
    using System.Collections.Generic;
    using System.Data;

    public partial class QuickQuery
    {
        private T WithReturn<T>(string sql, IDictionary<string, object> parameters)
        {
            using (var connection = _connectionProvider.Provide())
            using (var command = connection.GetCommandWithParametersSet(sql, parameters))
            {
                return (T)command.ExecuteScalar();
            }
        }

        private DataTable WithReturn(string sql, IDictionary<string, object> parameters)
        {
            using (var connection = _connectionProvider.Provide())
            using (var command = connection.GetCommandWithParametersSet(sql, parameters))
            {
                return _dataTableFiller.Fill(command);
            }
        }

        private DataTable WithReturn(int n, string sql, bool acceptsLess, IDictionary<string, object> parameters)
        {
            using (var connection = _connectionProvider.Provide())
            using (var command = connection.GetCommandWithParametersSet(sql, parameters))
            {
                var dataTable = _dataTableFiller.Fill(command);
                var rows = dataTable.Rows.Count;
                if (rows > n || (!acceptsLess && rows != n)) return dataTable;
                else throw new UnexpectedNumberOfRowsSelected(command, rows);
            }
        }
    }
}