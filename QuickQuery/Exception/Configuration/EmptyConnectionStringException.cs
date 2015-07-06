namespace QckQuery.Exception.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Exception thrown when an empty connection string is found.
    /// </summary>
    public class EmptyConnectionStringException : QuickQueryException
    {
        internal EmptyConnectionStringException(ConnectionStringSettings cs)
            : base(string.Format("The connection string \"{0}\" is empty.", cs.Name)) { }
    }
}
