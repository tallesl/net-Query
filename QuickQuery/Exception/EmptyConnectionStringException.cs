namespace QckQuery.Exception
{
    /// <summary>
    /// Exception thrown when a read connection string is empty.
    /// </summary>
    public class EmptyConnectionStringException : QuickQueryException
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="connectionStringName">The giving connection string name that it's empty</param>
        internal EmptyConnectionStringException(string connectionStringName) :
            base("The " + connectionStringName + " connection string is empty.") { }
    }
}
