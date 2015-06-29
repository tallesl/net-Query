namespace QckQuery.Exception.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Exception thrown when a connection string with the provided name doesn't exist.
    /// </summary>
    public class NoSuchConnectionStringException : QuickQueryException
    {
        internal NoSuchConnectionStringException(ConnectionStringSettings cs) :
            base("There's no \"" + cs.Name + "\" connection string.") { }
    }
}
