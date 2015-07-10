namespace QckQuery
{
    using QckQuery.DataAccess;
    using QckQuery.Exceptions.Querying;
    using System.Collections.Generic;
    using System.Data;

    public partial class QuickQuery
    {
        private DataTable WithReturnSelectingNRows(
            string sql, int n, bool acceptsLess, IDictionary<string, object> parameters)
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