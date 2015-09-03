namespace QckQuery
{
    using QckQuery.Exceptions;

    public partial class QuickQuery
    {
        /// <summary>
        /// Runs the given query giving no return.
        /// Throws UnexpectedNumberOfRowsAffected if more than one or none row is affected.
        /// It uses DbCommand.ExecuteNonQuery underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <exception cref="UnexpectedNumberOfRowsAffected">
        /// If more than one or none row is affected
        /// </exception>
        public void ChangeExactlyOne(string sql, object parameters = null)
        {
            ChangeExactly(1, sql, parameters);
        }
    }
}
