namespace QckQuery.DataAccess
{
    using System;
    using System.Data.Common;
    using QckQuery.Exception.Querying;

    internal static class ConnectionExtension
    {
        internal static DbCommand GetCommand(this DbConnection connection, string sql, string[] parameters)
        {
            var command = connection.CreateCommand();
            command.CommandText = sql;
            if (parameters != null)
            {
                CheckParameters(parameters);
                SetParameters(command, parameters);
            }
            return command;
        }

        private static void CheckParameters(string[] parameters)
        {
            if (parameters.Length % 2 != 0) throw new OddParametersException(parameters.Length);
        }

        private static void SetParameters(DbCommand command, string[] parameters)
        {
            for (var i = 0; i < parameters.Length; )
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = parameters[i++];
                parameter.Value = (object)parameters[i++] ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }
        }
    }
}
