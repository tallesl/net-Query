namespace QckQuery.Exception
{
    /// <summary>
    /// Exception thrown when a connection string with the provided name doesn't exist.
    /// </summary>
    public class NoSuchConnectionStringException : QuickQueryException
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="connectionStringName">The given connection string name that doesn't exist</param>
        internal NoSuchConnectionStringException(string connectionStringName) :
            base("There's no " + connectionStringName + " connection string.") { }
    }
}
