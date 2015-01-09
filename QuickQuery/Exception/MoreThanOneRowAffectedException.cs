namespace QckQuery.Exception
{
    /// <summary>
    /// Exception thrown when a query that shouldn't affect more than one row is executed.
    /// </summary>
    public class MoreThanOneRowAffectedException : QuickQueryException
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="sql">SQL that affected more than one row</param>
        internal MoreThanOneRowAffectedException(string sql) :
            base("The following query was rolled back because it shouldn't affect more than one row: " +
                sql + ".") { }
    }
}
