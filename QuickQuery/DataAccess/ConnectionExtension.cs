namespace QckQuery.DataAccess
{
    using System;
    using System.Data.Common;
    using QckQuery.Exception.Querying;

    internal static class ConnectionExtension
    {
        internal static DbCommand GetCommand(this DbConnection connection, string sql, object[] parameters)
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

        private static void CheckParameters(object[] parameters)
        {
            if (parameters.Length % 2 != 0) throw new OddParametersException(parameters.Length);
            for (var i = 0; i < parameters.Length; i += 2)
            {
                if (!(parameters[i] is string)) throw new ParameterNameNotString(parameters[i]);
            }
        }

        private static void SetParameters(DbCommand command, object[] parameters)
        {
            for (var i = 0; i < parameters.Length; )
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = (string)parameters[i++];
                parameter.Value = parameters[i++] ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }
        }
    }
}
