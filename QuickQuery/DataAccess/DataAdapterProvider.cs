namespace QckQuery.DataAccess
{
    using System.Configuration;
    using System.Data.Common;

    /// <summary>
    /// Provides database data adapters.
    /// </summary>
    internal class DataAdapterProvider
    {
        /// <summary>
        /// Factory used to create the data adapters.
        /// </summary>
        private readonly DbProviderFactory _factory;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="cs">Connection string to be use</param>
        internal DataAdapterProvider(ConnectionStringSettings cs)
        {
            _factory = DbProviderFactories.GetFactory(cs.ProviderName);
        }

        /// <summary>
        /// Yields a fresh data adapter.
        /// </summary>
        /// <returns>A fresh data adapter</returns>
        internal DbDataAdapter GetDataAdapter()
        {
            return _factory.CreateDataAdapter();
        }
    }
}
