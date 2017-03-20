namespace QueryLibrary
{
    using System;

    public sealed partial class Query
    {
        /// <summary>
        /// Runs the given query giving no return.
        /// Throws UnexpectedNumberOfRowsAffected if less than one row is affected.
        /// It uses DbCommand.ExecuteNonQuery underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <exception cref="UnexpectedNumberOfRowsAffectedException">
        /// If less than one row is affected
        /// </exception>
        public void ChangeNoLessThanOne(string sql, object parameters = null)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            ChangeNoLessThan(1, sql, parameters);
        }
    }
}
