namespace QckQuery
{
    using ConnectionStringReading;
    using QckQuery.DataAccess;
    using System;
    using System.Configuration;
    using System.Data.Common;

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
            var cs = ConnectionStringReader.Read(connectionStringName);
            InitializeMembers(cs);
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="cs">Connection string settings</param>
        public QuickQuery(ConnectionStringSettings cs)
        {
            if (cs == null) throw new ArgumentNullException("connectionString");
            ConnectionStringReader.Check(cs);
            InitializeMembers(cs);
        }

        private void InitializeMembers(ConnectionStringSettings cs)
        {
            var providerFactory = DbProviderFactories.GetFactory(cs.ProviderName);
            _connectionProvider = new ConnectionProvider(providerFactory, cs.ConnectionString);
            _dataAdapterProvider = new DataAdapterProvider(providerFactory);
            _dataTableFiller = new DataTableFiller(_dataAdapterProvider);
        }
    }
}
