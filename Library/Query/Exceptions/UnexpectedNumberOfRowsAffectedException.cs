namespace QueryLibrary
{
    using System;
    using System.Data.Common;
    using System.Globalization;

    /// <summary>
    /// Exception thrown when a query affects an unexpected number of rows.
    /// </summary>
    [Serializable]
    public class UnexpectedNumberOfRowsAffectedException : QueryException
    {
        internal UnexpectedNumberOfRowsAffectedException(DbCommand command, int n)
            : base(string.Format(
                CultureInfo.CurrentCulture,
                "The following query was rolled back because it affected {0} rows: {1}.",
                n, CommandFormatter.Format(command))) { }
    }
}
