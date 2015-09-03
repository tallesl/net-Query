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
        /// <summary>
        /// Flag indicating if it's to throw if a selected property is not found in the given type.
        /// </summary>
        public readonly bool Safe;

        private ConnectionProvider _connectionProvider;

        private DataAdapterProvider _dataAdapterProvider;

        private DataTableFiller _dataTableFiller;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="connectionStringName">Connection string name to be read from the .config file</param>
        /// <param name="safe">
        /// Flag indicating if it's to throw if a selected property is not found in the given type. Defaults to False.
        /// </param>
        public QuickQuery(string connectionStringName, bool safe = false)
        {
            var cs = ConnectionStringReader.Read(connectionStringName);
            InitializeMembers(cs);
            Safe = safe;
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="cs">Connection string settings</param>
        /// <param name="safe">
        /// Flag indicating if it's to throw is a selected property is not found in the given type. Defaults to False.
        /// </param>
        public QuickQuery(ConnectionStringSettings cs, bool safe = false)
        {
            if (cs == null) throw new ArgumentNullException("connectionString");
            ConnectionStringReader.Check(cs);
            InitializeMembers(cs);
            Safe = safe;
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
