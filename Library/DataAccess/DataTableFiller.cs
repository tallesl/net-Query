namespace QueryLibrary.DataAccess
{
    using System.Data;
    using System.Data.Common;

    internal class DataTableFiller
    {
        private readonly DataAdapterProvider _dataAdapterProvider;

        internal DataTableFiller(DataAdapterProvider dataAdapterProvider)
        {
            _dataAdapterProvider = dataAdapterProvider;
        }

        internal DataTable Fill(DbCommand command)
        {
            using (var adapter = _dataAdapterProvider.Provide())
            {
                var dataTable = new DataTable();
                adapter.SelectCommand = command;
                adapter.Fill(dataTable);
                return dataTable;
            }
        }
    }
}
