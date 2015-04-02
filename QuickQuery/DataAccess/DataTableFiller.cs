namespace QckQuery.DataAccess
{
    using System.Data;
    using System.Data.Common;

    /// <summary>
    /// Fills data tables from commands.
    /// </summary>
    internal class DataTableFiller
    {
        /// <summary>
        /// Provides database data adapter.
        /// </summary>
        private readonly DataAdapterProvider _provider;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="provider">Data adapter provider to use</param>
        internal DataTableFiller(DataAdapterProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Fills a data table from the given command and returns it.
        /// </summary>
        /// <param name="command">Command to get the data from</param>
        /// <returns>A filled data table</returns>
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
