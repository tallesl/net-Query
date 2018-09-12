namespace QueryLibrary
{
    using System;
    using System.Data.Common;
    using System.Globalization;

    /// <summary>
    /// Exception thrown when a query affects an unexpected number of rows.
    /// </summary>
    public class UnexpectedNumberOfRowsAffectedException : QueryException
    {
        internal UnexpectedNumberOfRowsAffectedException(DbCommand command, int n)
            : base($"The following query was rolled back because it affected {n} rows: " +
            CommandFormatter.Format(command)) { }
    }
}