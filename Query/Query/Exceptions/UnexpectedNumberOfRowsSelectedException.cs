namespace QueryLibrary
{
    using System;
    using System.Data.Common;
    using System.Globalization;

    /// <summary>
    /// Exception thrown when a query selects an unexpected number of rows.
    /// </summary>
    public class UnexpectedNumberOfRowsSelectedException : QueryException
    {
        internal UnexpectedNumberOfRowsSelectedException(DbCommand command, int n)
            : base($"The following query selected an unexpected number of rows ({n}): " +
            CommandFormatter.Format(command)) { }
    }
}