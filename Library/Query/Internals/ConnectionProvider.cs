namespace QueryLibrary
{
    using System.Configuration;
    using System.Data.Common;

    internal class ConnectionProvider
    {
        private readonly string _cs;

        private readonly DbProviderFactory _factory;

        internal ConnectionProvider(DbProviderFactory providerFactory, string connectionString)
        {
            _cs = connectionString;
            _factory = providerFactory;
        }

        internal DbConnection Provide()
        {
            var connection = _factory.CreateConnection();
            connection.ConnectionString = _cs;
            connection.Open();
            return connection;
        }
    }
}
