namespace QckQuery.DataAccess
{
    using System.Data;
    using System.Data.Common;

    internal class DataTableFiller
    {
        private readonly DataAdapterProvider _provider;

        internal DataTableFiller(DataAdapterProvider provider)
        {
            _provider = provider;
        }

        internal DataTable Fill(DbCommand command)
        {
            using (var adapter = _provider.GetDataAdapter())
            {
                var dataTable = new DataTable();
                adapter.SelectCommand = command;
                adapter.Fill(dataTable);
                return dataTable;
            }
        }
    }
}
