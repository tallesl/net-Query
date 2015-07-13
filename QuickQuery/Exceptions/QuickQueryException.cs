namespace QckQuery.Exceptions
{
    using System;

    /// <summary>
    /// QuickQuery's base exception class.
    /// </summary>
    [Serializable]
    public abstract class QuickQueryException : Exception
    {
        internal QuickQueryException(string message) : base(message) { }
    }
}
