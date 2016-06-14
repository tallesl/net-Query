namespace QueryLibrary
{
    using DictionaryLibrary;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Diagnostics.CodeAnalysis;

    internal static class ParameterSetter
    {
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        internal static DbCommand GetCommand(
            this DbConnection connection, string sql, object parameters, QueryOptions options)
        {
            var command = connection.CreateCommand();

            // This generates CA2100 warning.
            // Supressing the warning because the injection it's still possible but only if the library is misused
            // (concatenating the SQL string on their own instead of passing parameters).
            command.CommandText = sql;

            if (parameters != null)
            {
                foreach (var kvp in DictionaryMaker.Make(parameters))
                {
                    var name = kvp.Key;
                    var value = kvp.Value;
                    var enumerable = value as IEnumerable;

                    if (!options.ArrayAsInClause || enumerable == null || value is string)
                        SetSingleParameter(command, name, value, options.EnumAsString);
                    else
                        SetCollectionParameter(command, name, enumerable, options.EnumAsString);
                }
            }

            return command;
        }

        private static void SetSingleParameter(DbCommand command, string name, object value, bool enumAsString)
        {
            var parameter = command.CreateParameter();

            parameter.ParameterName = name;
            parameter.Value = (enumAsString && value is Enum) ? value.ToString() : (value ?? DBNull.Value);

            command.Parameters.Add(parameter);
        }

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        private static void SetCollectionParameter(
            DbCommand command, string name, IEnumerable collection, bool enumAsString)
        {
            var parameters = new List<string>();
            var j = 0;

            foreach (var el in collection)
            {
                var currentName = name + j++;
                SetSingleParameter(command, currentName, el, enumAsString);
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
