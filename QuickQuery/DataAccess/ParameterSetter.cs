namespace QckQuery.DataAccess
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Diagnostics.CodeAnalysis;

    internal static class ParameterSetter
    {
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        internal static DbCommand GetCommandWithParametersSet(
            this DbConnection connection, string sql, IDictionary<string, object> parameters)
        {
            var command = connection.CreateCommand();

            // This generates CA2100 warning.
            // Supressing the warning because the injection it's still possible but only if the library is misused
            // (concatenating the SQL string on their own instead of passing parameters).
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

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
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

            // This generates CA2100 warning.
            // If one of the collection itens is an input from an user, that could be a potential injection (since we
            // have to concatenated on our own).
            // Supressing the warning because we warn about this on the library documentation.
            var clause = string.Join(",", parameters);
            command.CommandText = command.CommandText.Replace("@" + name, clause);
        }
    }
}
