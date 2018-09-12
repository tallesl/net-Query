namespace QueryLibrary
{
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    internal class DataTableFiller
    {
        private readonly DataAdapterProvider _dataAdapterProvider;

        internal DataTableFiller(DataAdapterProvider dataAdapterProvider) => _dataAdapterProvider = dataAdapterProvider;

        internal DataTable Fill(DbCommand command)
        {
            var dataTable = new DataTable();
            dataTable.Locale = CultureInfo.CurrentCulture;

            var adapter = _dataAdapterProvider.Provide();
            adapter.SelectCommand = command;
            adapter.Fill(dataTable);

            return dataTable;
        }
    }
}