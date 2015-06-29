namespace QckQuery.Formatting
{
    using System.Data.Common;
    using System.Text;

    internal static class CommandFormatter
    {
        internal static string GetFormattedSql(this DbCommand command)
        {
            var sql = new StringBuilder(command.CommandText);
            foreach (DbParameter p in command.Parameters) sql.Replace(p.ParameterName, p.Value.ToString());
            return sql.ToString();
        }
    }
}
