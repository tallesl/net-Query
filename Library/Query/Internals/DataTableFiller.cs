namespace QueryLibrary
{
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    internal class DataTableFiller
    {
        private readonly DataAdapterProvider _dataAdapterProvider;

        internal DataTableFiller(DataAdapterProvider dataAdapterProvider)
        {
            _dataAdapterProvider = dataAdapterProvider;
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "http://stackoverflow.com/a/18869167/1316620")]
        internal DataTable Fill(DbCommand command)
        {
            var adapter = _dataAdapterProvider.Provide();
            var dataTable = new DataTable();
            dataTable.Locale = CultureInfo.CurrentCulture;
            adapter.SelectCommand = command;
            adapter.Fill(dataTable);
            return dataTable;
        }
    }
}
