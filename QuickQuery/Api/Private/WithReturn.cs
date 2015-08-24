namespace QckQuery
{
    using QckQuery.DataAccess;
    using QckQuery.Exceptions;
    using System.Data;

    public partial class QuickQuery
    {
        private T WithReturn<T>(string sql, object parameters)
        {
            using (var connection = _connectionProvider.Provide())
            using (var command = connection.GetCommandWithParametersSet(sql, parameters))
            {
                return (T)command.ExecuteScalar();
            }
        }

        private DataTable WithReturn(string sql, object parameters)
        {
            using (var connection = _connectionProvider.Provide())
            using (var command = connection.GetCommandWithParametersSet(sql, parameters))
            {
                return _dataTableFiller.Fill(command);
            }
        }

        private DataTable WithReturn(int n, string sql, bool acceptsLess, object parameters)
        {
            using (var connection = _connectionProvider.Provide())
            using (var command = connection.GetCommandWithParametersSet(sql, parameters))
            {
                var dataTable = _dataTableFiller.Fill(command);
                var selected = dataTable.Rows.Count;
                if (selected == n || (acceptsLess && selected < n)) return dataTable;
                else throw new UnexpectedNumberOfRowsSelected(command, selected);
            }
        }
    }
}