namespace QueryLibrary
{
    using System;
    using System.Configuration;
    using System.Data.Common;

    /// <summary>
    /// A simplistic ADO.NET wrapper.
    /// </summary>
    public sealed partial class Query : IDisposable
    {
        private readonly QueryOptions _options;

        private ConnectionProvider _connectionProvider;

        private DataAdapterProvider _dataAdapterProvider;

        private DataTableFiller _dataTableFiller;

        private ParameterSetter _parameterSetter;

        private DbConnection _currentConnection = null;

        private DbTransaction _currentTransaction = null;

        private object _connectionLock = new object();

        private bool _disposed = false;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="connectionString">Connection string to use</param>
        /// <param name="providerName">Data provider to use</param>
        public Query(string connectionString, string providerName)
            : this(connectionString, providerName, new QueryOptions()) { }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="connectionString">Connection string to use</param>
        /// <param name="providerName"></param>
        /// <param name="options">Configuration options</param>
        public Query(string connectionString, string providerName, QueryOptions options)
        {
            _options = options;
            InitializeMembers(connectionString, providerName);
        }

        private void InitializeMembers(string connectionString, string providerName)
        {
            var providerFactory = DbProviderFactories.GetFactory(providerName);

            _connectionProvider = new ConnectionProvider(connectionString, providerFactory);
            _dataAdapterProvider = new DataAdapterProvider(providerFactory);
            _dataTableFiller = new DataTableFiller(_dataAdapterProvider);
            _parameterSetter = new ParameterSetter(_options);
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
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (_options.ManualClosing)
                CloseRegardless();
        }

        private void CloseIfNeeded()
        {
            if (!_options.ManualClosing)
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

        /// <summary>
        /// Disposes any open underlying connection or transaction.
        /// Only need when ManualClosing is set to True.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            CloseRegardless();
            _disposed = true;
        }
    }
}