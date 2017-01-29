namespace QueryLibrary
{
    using System;
    using System.Data.Common;
    using System.Globalization;

    /// <summary>
    /// Exception thrown when a query selects an unexpected number of rows.
    /// </summary>
    [Serializable]
    public class UnexpectedNumberOfRowsSelectedException : QueryException
    {
        internal UnexpectedNumberOfRowsSelectedException(DbCommand command, int n)
            : base(string.Format(
                CultureInfo.CurrentCulture,
                "The following query selected an unexpected number of rows ({0}): {1}.",
                n, CommandFormatter.Format(command))) { }
    }
}
