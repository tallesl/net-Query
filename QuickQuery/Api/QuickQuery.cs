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

        private DbConnection _currentConnection = null;

        private DbTransaction _currentTransaction = null;

        private object _connectionLock = new object();

        private readonly QuickQueryConfiguration _configuration;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="connectionStringName">Connection string name to be read from the .config file</param>
        public QuickQuery(string connectionStringName) : this(connectionStringName, new QuickQueryConfiguration()) { }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="connectionStringName">Connection string name to be read from the .config file</param>
        /// <param name="configuration">Configuration options</param>
        public QuickQuery(string connectionStringName, QuickQueryConfiguration configuration)
        {
            _configuration = configuration;
            var cs = ConnectionStringReader.Read(connectionStringName);
            InitializeMembers(cs);
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="cs">Connection string settings</param>
        public QuickQuery(ConnectionStringSettings cs) : this(cs, new QuickQueryConfiguration()) { }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="cs">Connection string settings</param>
        /// <param name="configuration">Configuration options</param>
        public QuickQuery(ConnectionStringSettings cs, QuickQueryConfiguration configuration)
        {
            if (cs == null)
                throw new ArgumentNullException("connectionString");

            _configuration = configuration;
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

        private DbConnection OpenConnection
        {
            get
            {
                lock (_connectionLock)
                {
                    if (_currentConnection == null)
                        _currentConnection = _connectionProvider.Provide();

                    return _currentConnection;
                }
            }
        }

        private DbTransaction OpenTransaction
        {
            get
            {
                lock (_connectionLock)
                {
                    if (_currentTransaction == null)
                        _currentTransaction = OpenConnection.BeginTransaction();

                    return _currentTransaction;
                }
            }
        }

        /// <summary>
        /// Closes and commit any underlying connection and transaction that were open automatically.
        /// Does nothing if ManualClosing is False.
        /// </summary>
        public void Close()
        {
            if (_configuration.ManualClosing)
                CloseRegardless();
        }

        private void CloseIfNeeded()
        {
            if (!_configuration.ManualClosing)
                CloseRegardless();
        }

        private void CloseRegardless(bool rollback = false)
        {
            lock (_connectionLock)
            {
                if (_currentConnection == null)
                    return;

                if (_currentTransaction != null)
                {
                    if (rollback)
                        _currentTransaction.Rollback();
                    else
                        _currentTransaction.Commit();

                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }

                _currentConnection.Dispose();
                _currentConnection = null;
            }
        }
    }
}
