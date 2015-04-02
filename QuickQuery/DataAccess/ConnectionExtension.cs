namespace QckQuery.DataAccess
{
    using System;
    using System.Data.Common;
    using QckQuery.Exception.Querying;

    /// <summary>
    /// Provides database commands.
    /// </summary>
    internal static class ConnectionExtension
    {
        /// <summary>
        /// Sets up a command.
        /// </summary>
        /// <param name="connection">Command's connection</param>
        /// <param name="sql">Command's SQL query</param>
        /// <param name="parameters">Parameter pairs (key and value) of the SQL query</param>
        /// <returns>The set up command</returns>
        /// <exception cref="OddParametersException">If an odd number of parameters is passed</exception>
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

        /// <summary>
        /// Checks for an odd number of parameters.
        /// </summary>
        /// <param name="parameters">Parameters to check</param>
        /// <exception cref="OddParametersException">If an odd number of parameters is passed</exception>
        private static void CheckParameters(string[] parameters)
        {
            if (parameters.Length % 2 != 0) throw new OddParametersException(parameters.Length);
        }

        /// <summary>
        /// Sets up the command parameters.
        /// </summary>
        /// <param name="command">Command to set up</param>
        /// <param name="parameters">Parameter pairs (key and value) of the SQL query</param>
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
