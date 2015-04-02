namespace QckQuery.DataAccess
{
    using System.Configuration;
    using System.Data.Common;

    /// <summary>
    /// Provides database connections.
    /// </summary>
    internal class ConnectionProvider
    {
        /// <summary>
        /// Connection string.
        /// </summary>
        private readonly string _cs;

        /// <summary>
        /// Factory used to create the connections.
        /// </summary>
        private readonly DbProviderFactory _factory;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="cs">Connection string to be use</param>
        internal ConnectionProvider(ConnectionStringSettings cs)
        {
            _cs = cs.ConnectionString;
            _factory = DbProviderFactories.GetFactory(cs.ProviderName);
        }

        /// <summary>
        /// Yields a fresh open connection.
        /// </summary>
        /// <returns>A fresh open connection</returns>
        internal DbConnection GetOpenConnection()
        {
            var connection = _factory.CreateConnection();
            connection.ConnectionString = _cs;
            connection.Open();
            return connection;
        }
    }
}
