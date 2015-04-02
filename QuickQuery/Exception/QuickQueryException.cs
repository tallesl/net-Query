namespace QckQuery.Exception
{
    using System;

    /// <summary>
    /// QuickQuery's base exception class.
    /// </summary>
    public abstract class QuickQueryException : Exception
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="message">Error message</param>
        internal QuickQueryException(string message) : base(message) { }
    }
}
