namespace QckQuery
{
    using System.Configuration;
    using QckQuery.DataAccess;
    using QckQuery.Exception.Configuration;

    /// <summary>
    /// Performs a *quick query* to a database.
    /// </summary>
    public partial class QuickQuery
    {
        /// <summary>
        /// Provides database connection.
        /// </summary>
        private readonly ConnectionProvider _connectionProvider;

        /// <summary>
        /// Provides database data adapter.
        /// </summary>
        private readonly DataAdapterProvider _dataAdapterProvider;

        /// <summary>
        /// Fills data table from command.
        /// </summary>
        private readonly DataTableFiller _dataTableFiller;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="connectionStringName">Connection string name to be read from the .config file</param>
        public QuickQuery(string connectionStringName)
        {
            var cs = ConfigurationManager.ConnectionStrings[connectionStringName];
            CheckConnectionString(cs);
            _connectionProvider = new ConnectionProvider(cs);
            _dataAdapterProvider = new DataAdapterProvider(cs);
            _dataTableFiller = new DataTableFiller(_dataAdapterProvider);
        }

        /// <summary>
        /// Checks if the given connection string is malformed.
        /// </summary>
        /// <param name="cs">Connection string settings</param>
        /// <exception cref="NoSuchConnectionStringException">If didn't find the connection string</exception>
        /// <exception cref="EmptyConnectionStringException">If the found connection string is empty</exception>
        /// <exception cref="EmptyProviderNameException">If the found provider name is empty</exception>
        private void CheckConnectionString(ConnectionStringSettings cs)
        {
            if (cs == null) throw new NoSuchConnectionStringException(cs);
            if (string.IsNullOrWhiteSpace(cs.ConnectionString)) throw new EmptyConnectionStringException(cs);
            if (string.IsNullOrWhiteSpace(cs.ProviderName)) throw new EmptyProviderNameException(cs);
        }
    }
}
