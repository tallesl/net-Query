namespace QckQuery.DataAccess
{
    using System.Configuration;
    using System.Data.Common;

    internal class DataAdapterProvider
    {
        private readonly DbProviderFactory _factory;

        internal DataAdapterProvider(ConnectionStringSettings cs)
        {
            _factory = DbProviderFactories.GetFactory(cs.ProviderName);
        }

        internal DbDataAdapter GetDataAdapter()
        {
            return _factory.CreateDataAdapter();
        }
    }
}
