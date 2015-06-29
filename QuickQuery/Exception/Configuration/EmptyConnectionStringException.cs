namespace QckQuery.Exception.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Exception thrown when an empty connection string is found.
    /// </summary>
    public class EmptyConnectionStringException : QuickQueryException
    {
        internal EmptyConnectionStringException(ConnectionStringSettings cs) :
            base("The connection string \"" + cs.Name + "\" is empty.") { }
    }
}
