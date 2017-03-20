namespace QueryLibrary
{
    using System.Configuration;
    using System.Data.Common;

    internal class DataAdapterProvider
    {
        private readonly DbProviderFactory _factory;

        internal DataAdapterProvider(DbProviderFactory factory)
        {
            _factory = factory;
        }

        internal DbDataAdapter Provide()
        {
            return _factory.CreateDataAdapter();
        }
    }
}
