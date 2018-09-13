namespace QueryLibrary
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// A simplistic ADO.NET wrapper.
    /// </summary>
    public sealed partial class Query : IDisposable
    {
        private readonly string _connectionString;

        private readonly bool _manualClosing;

        private readonly bool _safe;

        private readonly DbProviderFactory _providerFactory;

        private readonly ParameterSetter _parameterSetter;

        private DbConnection _currentConnection = null;

        private DbTransaction _currentTransaction = null;

        private object _connectionLock = new object();

        private bool _disposed = false;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="connectionString">Connection string to use</param>
        /// <param name="providerName">Data provider to use</param>
        /// <param name="enumAsString">Treat enum values as strings (ToString)</param>
        /// <param name="manualClosing">
        /// Connection/transaction closing 'manually' instead of automatically on each call
        /// </param>
        /// <param name="safe">Throws when a selected a property is not found in the given type</param>
        /// <param name="timeout">Optional DBCommand.CommandTimeout value</param>
        public Query(string connectionString, string providerName, bool enumAsString = false,
            bool manualClosing = false, bool safe = false, int? timeout = null)
        {
            _providerFactory = DbProviderFactories.GetFactory(providerName);

            _connectionString = connectionString;
            _manualClosing = manualClosing;
            _safe = safe;

            _parameterSetter = new ParameterSetter(enumAsString, timeout);
        }

        private DbConnection OpenConnection
        {
            get
            {
                lock (_connectionLock)
                {
                    if (_currentConnection == null)
                    {
                        _currentConnection = _providerFactory.CreateConnection();
                        _currentConnection.ConnectionString = _connectionString;
                        _currentConnection.Open();
                    }

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
        /// Does nothing if manualClosing is set to False.
        /// </summary>
        public void Close()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (_manualClosing)
                CloseRegardless();
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

        internal static string FormatSql(DbCommand command)
        {
            var sql = new StringBuilder(command.CommandText);

            foreach (DbParameter p in command.Parameters)
                sql.Replace('@' + p.ParameterName, p.Value.ToString());

            return sql.ToString();
        }

        private void CloseIfNeeded()
        {
            if (!_manualClosing)
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

        private DataTable FillDataTable(DbCommand command)
        {
            var dataTable = new DataTable();
            dataTable.Locale = CultureInfo.CurrentCulture;

            var adapter = _providerFactory.CreateDataAdapter();
            adapter.SelectCommand = command;
            adapter.Fill(dataTable);

            return dataTable;
        }
    }
}