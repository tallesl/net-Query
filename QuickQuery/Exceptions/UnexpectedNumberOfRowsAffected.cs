namespace QckQuery.Exceptions
{
    using DbCommandFormatting;
    using System;
    using System.Data.Common;

    /// <summary>
    /// Exception thrown when a query affects an unexpected number of rows.
    /// </summary>
    [Serializable]
    public class UnexpectedNumberOfRowsAffected : QuickQueryException
    {
        internal UnexpectedNumberOfRowsAffected(DbCommand command, int n)
            : base(string.Format("The following query was rolled back because it affected {0} rows: {1}.",
                n, DbCommandFormatter.Format(command))) { }
    }
}
