namespace QueryLibrary
{
    using QueryLibrary.Exceptions;

    public partial class Query
    {
        /// <summary>
        /// Runs the given query giving no return.
        /// Throws UnexpectedNumberOfRowsAffected if more than one row is affected.
        /// It uses DbCommand.ExecuteNonQuery underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <exception cref="UnexpectedNumberOfRowsAffected">
        /// If more than one row is affected
        /// </exception>
        public void ChangeNoMoreThanOne(string sql, object parameters = null)
        {
            ChangeNoMoreThan(1, sql, parameters);
        }
    }
}
