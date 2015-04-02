namespace QckQuery.Formatting
{
    using System.Data.Common;
    using System.Text;

    /// <summary>
    /// Formats DbCommands.
    /// </summary>
    internal static class CommandFormatter
    {
        /// <summary>
        /// Formats the underlying SQL of the given DbCommand.
        /// </summary>
        /// <param name="command">DbCommand to format</param>
        /// <returns>The SQL command</returns>
        internal static string GetFormattedSql(this DbCommand command)
        {
            var sql = new StringBuilder(command.CommandText);
            foreach (DbParameter p in command.Parameters) sql.Replace(p.ParameterName, p.Value.ToString());
            return sql.ToString();
        }
    }
}
