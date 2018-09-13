namespace QueryLibrary
{
    using System;
    using System.Data.Common;
    using System.Text;

    /// <summary>
    /// Query's base exception class.
    /// </summary>
    public abstract class QueryException : Exception
    {
        internal QueryException(string message) : base(message) { }
    }
}