namespace QueryLibrary
{
    public partial class Query
    {
        /// <summary>
        /// Runs the given query giving no return.
        /// It uses DbCommand.ExecuteNonQuery underneath.
        /// </summary>
        /// <param name="sql">Query to run</param>
        /// <param name="parameters">Parameters names and values pairs</param>
        public void Change(string sql, object parameters = null)
        {
            WithoutReturn(sql, parameters);
        }
    }
}
