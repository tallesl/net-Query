namespace QckQuery.DataAccess
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Common;

    internal static class ParameterSetter
    {
        internal static DbCommand GetCommandWithParametersSet(
            this DbConnection connection, string sql, IDictionary<string, object> parameters)
        {
            var command = connection.CreateCommand();
            command.CommandText = sql;
            SetParameters(command, parameters);
            return command;
        }

        private static void SetParameters(DbCommand command, IDictionary<string, object> parameters)
        {
            foreach (var kvp in parameters)
            {
                var name = kvp.Key;
                var value = kvp.Value;

                if (value is IEnumerable && !(value is string))
                {
                    SetCollectionParameter(command, name, (IEnumerable)value);
                }
                else
                {
                    SetSingleParameter(command, name, value);
                }
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
