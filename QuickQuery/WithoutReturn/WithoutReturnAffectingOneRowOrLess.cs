namespace QckQuery
{
    using QckQuery.Exceptions.Querying;

    public partial class QuickQuery
    {
        /// <summary>
        /// Runs the given query giving no return.
        /// Throws UnexpectedNumberOfRowsAffected if more than one row was affected.
        /// It uses DbCommand.ExecuteNonQuery underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <exception cref="UnexpectedNumberOfRowsAffected">If more than one row was affected</exception>
        public void WithoutReturnAffectingOneRowOrLess(string sql, object parameters)
        {
            WithoutReturnAffectingNRowsOrLess(sql, 1, parameters);
        }

        /// <summary>
        /// Runs the given query giving no return.
        /// Throws UnexpectedNumberOfRowsAffected if more than one row was affected.
        /// It uses DbCommand.ExecuteNonQuery underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <exception cref="UnexpectedNumberOfRowsAffected">If more than one row was affected</exception>
        public void WithoutReturnAffectingOneRowOrLess(string sql, params object[] parameters)
        {
            WithoutReturnAffectingNRowsOrLess(sql, 1, parameters);
        }
    }
}
