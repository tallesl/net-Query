namespace QckQuery.Formatting
{
    using System.Data.Common;
    using System.Text;

    internal static class SqlFormatter
    {
        internal static string Format(DbCommand command)
        {
            var sql = new StringBuilder(command.CommandText);
            foreach (DbParameter p in command.Parameters) sql.Replace(p.ParameterName, p.Value.ToString());
            return sql.ToString();
        }
    }
}
