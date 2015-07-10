namespace QckQuery
{
    public partial class QuickQuery
    {
        /// <summary>
        /// Runs the given query giving no return.
        /// Throws UnexpectedNumberOfRowsAffected if none or more than one row was affected.
        /// It uses DbCommand.ExecuteNonQuery underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <exception cref="UnexpectedNumberOfRowsAffected">If none or more than one row was affected</exception>
        public void WithoutReturnAffectingExactlyOneRow(string sql, object parameters)
        {
            WithoutReturnAffectingExactlyNRows(sql, 1, parameters);
        }

        /// <summary>
        /// Runs the given query giving no return.
        /// Throws UnexpectedNumberOfRowsAffected if none or more than one row was affected.
        /// It uses DbCommand.ExecuteNonQuery underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <exception cref="UnexpectedNumberOfRowsAffected">If none or more than one row was affected</exception>
        public void WithoutReturnAffectingExactlyOneRow(string sql, params object[] parameters)
        {
            WithoutReturnAffectingExactlyNRows(sql, 1, parameters);
        }
    }
}
