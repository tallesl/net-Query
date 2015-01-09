namespace QckQuery.Exception
{
    /// <summary>
    /// QuickQuery's base exception class.
    /// </summary>
    public abstract class QuickQueryException : System.Exception
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="message">Error message</param>
        internal QuickQueryException(string message) : base(message) { }
    }
}
