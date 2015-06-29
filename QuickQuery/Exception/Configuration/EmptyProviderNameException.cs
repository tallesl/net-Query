namespace QckQuery.Exception.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Exception thrown when an empty provider name is found.
    /// </summary>
    public class EmptyProviderNameException : QuickQueryException
    {
        internal EmptyProviderNameException(ConnectionStringSettings cs) :
            base("The provider name of the connection string \"" + cs.Name + "\" is empty.") { }
    }
}
