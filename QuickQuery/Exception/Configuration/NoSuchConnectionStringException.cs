namespace QckQuery.Exception.Configuration
{
    /// <summary>
    /// Exception thrown when a connection string with the provided name doesn't exist.
    /// </summary>
    public class NoSuchConnectionStringException : QuickQueryException
    {
        internal NoSuchConnectionStringException(string name)
            : base(string.Format("There's no \"{0}\" connection string.", name)) { }
    }
}
