namespace QckQuery.DataAccess
{
    using QckQuery.Exception.Querying;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Common;

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
                var name = (string)parameters[i++];
                var value = parameters[i++];

                if (value is IEnumerable && !(value is string)) SetCollectionParameter(command, name, (IEnumerable)value);
                else SetSingleParameter(command, name, value);
            }
        }

        private static void SetSingleParameter(DbCommand command, string name, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value ?? DBNull.Value;
            command.Parameters.Add(parameter);
        }

        private static void SetCollectionParameter(DbCommand command, string name, IEnumerable collection)
        {
            var parameters = new List<string>();
            var j = 0;

            foreach (var el in collection)
            {
                var currentName = name + j++;
                SetSingleParameter(command, currentName, el);
                parameters.Add("@" + currentName);
            }

            var clause = string.Join(",", parameters);
            command.CommandText = command.CommandText.Replace("@" + name, clause);
        }
    }
}
