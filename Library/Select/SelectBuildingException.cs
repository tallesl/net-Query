namespace QueryLibrary
{
    using System;

    /// <summary>
    /// Exception thrown when building a SELECT clause.
    /// </summary>
    [Serializable]
    public class SelectBuildingException : Exception
    {
        internal SelectBuildingException(string message) : base(message) { }
    }
}
