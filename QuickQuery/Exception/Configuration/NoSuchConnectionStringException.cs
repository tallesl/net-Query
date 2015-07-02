namespace QckQuery.Exception.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Exception thrown when a connection string with the provided name doesn't exist.
    /// </summary>
    public class NoSuchConnectionStringException : QuickQueryException
    {
        internal NoSuchConnectionStringException(string name) :
            base("There's no \"" + name + "\" connection string.") { }
    }
}
