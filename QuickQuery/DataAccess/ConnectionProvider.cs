namespace QckQuery.DataAccess
{
    using System.Configuration;
    using System.Data.Common;

    internal class ConnectionProvider
    {
        private readonly string _cs;

        private readonly DbProviderFactory _factory;

        internal ConnectionProvider(ConnectionStringSettings cs)
        {
            _cs = cs.ConnectionString;
            _factory = DbProviderFactories.GetFactory(cs.ProviderName);
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
