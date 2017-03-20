namespace QueryLibrary
{
    using System;

    /// <summary>
    /// Query's base exception class.
    /// </summary>
    [Serializable]
    public abstract class QueryException : Exception
    {
        internal QueryException(string message) : base(message) { }
    }
}
