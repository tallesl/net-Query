namespace QueryLibrary
{
    using System;

    public sealed partial class Query
    {
        /// <summary>
        /// Runs the given query giving no return.
        /// Throws UnexpectedNumberOfRowsAffected if the number of affected rows is less than N.
        /// It uses DbCommand.ExecuteNonQuery underneath.
        /// </summary>
        /// <param name="n">Minimum of affected rows to ensure</param>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        /// <exception cref="UnexpectedNumberOfRowsAffectedException">
        /// If the number of affected rows is less than N
        /// </exception>
        public void ChangeNoLessThan(int n, string sql, object parameters = null)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            WithoutReturn(sql, parameters, CountValidationEnum.NoLessThan, n);
        }
    }
}