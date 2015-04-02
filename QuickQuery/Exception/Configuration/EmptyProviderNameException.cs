namespace QckQuery.Exception.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Exception thrown when an empty provider name is found.
    /// </summary>
    public class EmptyProviderNameException : QuickQueryException
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="cs">The connection string with the empty provider</param>
        internal EmptyProviderNameException(ConnectionStringSettings cs) :
            base("The provider name of the connection string \"" + cs.Name + "\" is empty.") { }
    }
}
