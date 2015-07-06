namespace QckQuery.Exception.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Exception thrown when an empty provider name is found.
    /// </summary>
    public class EmptyProviderNameException : QuickQueryException
    {
        internal EmptyProviderNameException(ConnectionStringSettings cs)
            : base(string.Format("The provider name of the connection string \"{0}\" is empty.", cs.Name)) { }
    }
}
