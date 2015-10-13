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
        /// Flag indicating if enum values should be treated as strings (ToString).
        /// </summary>
        public bool EnumAsString { get; set; }

        /// <summary>
        /// Flag indicating if closing the connection/transaction should be done manually by the library consumer.
        /// If False it automatically opens/closes on each API call.
        /// </summary>
        public bool ManualClosing { get; set; }

        /// <summary>
        /// Flag indicating if it's to throw if a selected property is not found in the given type.
        /// </summary>
        public bool Safe { get; set; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="cs">Connection string settings</param>
        public QuickQuery(ConnectionStringSettings cs)
        {
            if (cs == null)
                throw new ArgumentNullException("connectionString");

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
            if (ManualClosing)
                CloseRegardless();
        }

        private void CloseIfNeeded()
        {
            if (!ManualClosing)
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
