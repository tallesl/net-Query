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
        private readonly ConnectionProvider _connectionProvider;

        private readonly DataAdapterProvider _dataAdapterProvider;

        private readonly DataTableFiller _dataTableFiller;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="connectionStringName">Connection string name to be read from the .config file</param>
        public QuickQuery(string connectionStringName) :
            this(ConfigurationManager.ConnectionStrings[connectionStringName]) { }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="connectionString">Connection string settings</param>
        public QuickQuery(ConnectionStringSettings connectionString)
        {
            CheckConnectionString(connectionString);
            _connectionProvider = new ConnectionProvider(connectionString);
            _dataAdapterProvider = new DataAdapterProvider(connectionString);
            _dataTableFiller = new DataTableFiller(_dataAdapterProvider);
        }

        private void CheckConnectionString(ConnectionStringSettings cs)
        {
            if (cs == null) throw new NoSuchConnectionStringException(cs);
            if (string.IsNullOrWhiteSpace(cs.ConnectionString)) throw new EmptyConnectionStringException(cs);
            if (string.IsNullOrWhiteSpace(cs.ProviderName)) throw new EmptyProviderNameException(cs);
        }
    }
}
