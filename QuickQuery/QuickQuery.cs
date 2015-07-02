namespace QckQuery
{
    using QckQuery.DataAccess;
    using QckQuery.Exception.Configuration;
    using System;
    using System.Configuration;

    /// <summary>
    /// Performs a *quick query* to a database.
    /// </summary>
    public partial class QuickQuery
    {
        private ConnectionProvider _connectionProvider;

        private DataAdapterProvider _dataAdapterProvider;

        private DataTableFiller _dataTableFiller;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="connectionStringName">Connection string name to be read from the .config file</param>
        public QuickQuery(string connectionStringName)
        {
            if (connectionStringName == null) throw new ArgumentNullException("connectionStringName");

            var cs = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (cs == null) throw new NoSuchConnectionStringException(connectionStringName);

            InitializeMembers(cs);
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="connectionString">Connection string settings</param>
        public QuickQuery(ConnectionStringSettings connectionString)
        {
            if (connectionString == null) throw new ArgumentNullException("connectionString");
            InitializeMembers(connectionString);
        }

        private void InitializeMembers(ConnectionStringSettings cs)
        {
            if (string.IsNullOrWhiteSpace(cs.ConnectionString)) throw new EmptyConnectionStringException(cs);
            if (string.IsNullOrWhiteSpace(cs.ProviderName)) throw new EmptyProviderNameException(cs);

            _connectionProvider = new ConnectionProvider(cs);
            _dataAdapterProvider = new DataAdapterProvider(cs);
            _dataTableFiller = new DataTableFiller(_dataAdapterProvider);
        }
    }
}
