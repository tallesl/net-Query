namespace QueryLibrary
{
    using System;

    /// <summary>
    /// Exception thrown when building a SELECT clause.
    /// </summary>
    public class SelectBuildingException : Exception
    {
        internal SelectBuildingException(string message) : base(message) { }
    }
}