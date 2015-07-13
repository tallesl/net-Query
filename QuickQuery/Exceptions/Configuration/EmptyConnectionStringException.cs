namespace QckQuery.Exceptions.Configuration
{
    using System;
    using System.Configuration;

    /// <summary>
    /// Exception thrown when an empty connection string is found.
    /// </summary>
    [Serializable]
    public class EmptyConnectionStringException : QuickQueryException
    {
        internal EmptyConnectionStringException(ConnectionStringSettings cs)
            : base(string.Format("The connection string \"{0}\" is empty.", cs.Name)) { }
    }
}
