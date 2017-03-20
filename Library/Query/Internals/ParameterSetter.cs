namespace QueryLibrary
{
    using DictionaryLibrary;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;

    internal class ParameterSetter
    {
        private readonly QueryOptions _options;

        internal ParameterSetter(QueryOptions options)
        {
            _options = options;
        }

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        internal DbCommand GetCommand(DbConnection connection, string sql, object parameters)
        {
            var command = connection.CreateCommand();

            // This generates CA2100 warning.
            // Supressing the warning because the injection it's still possible but only if the library is misused
            // (concatenating the SQL string on their own instead of passing parameters).
            command.CommandText = sql;

            if (parameters != null)
            {
                foreach (var kvp in DictionaryMaker.MakeWithType(parameters))
                {
                    var name = kvp.Key;
                    var type = kvp.Value.Item1;
                    var value = kvp.Value.Item2;
                    var enumerable = value as IEnumerable;

                    if (!_options.ArrayAsInClause || enumerable == null || value is string)
                        SetSingleParameter(command, name, type, value);
                    else
                        SetCollectionParameter(command, name, enumerable);
                }
            }

            return command;
        }

        private void SetSingleParameter(DbCommand command, string name, Type type, object value)
        {
            var parameter = command.CreateParameter();

            parameter.ParameterName = name;
            parameter.DbType = type.ToDbType();
            parameter.Value = (_options.EnumAsString && value is Enum) ? value.ToString() : (value ?? DBNull.Value);

            // For some reason System.Data.DbType.Time is
            // "A type representing a SQL Server DateTime value. If you want to use a SQL Server time value, use Time."
            // https://msdn.microsoft.com/library/System.Data.DbType
            var sqlParameter = parameter as SqlParameter;
            if (sqlParameter != null && type == typeof(TimeSpan))
                sqlParameter.SqlDbType = SqlDbType.Time;

            command.Parameters.Add(parameter);
        }

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        private void SetCollectionParameter(DbCommand command, string name, IEnumerable collection)
        {
            var parameters = new List<string>();
            var j = 0;

            foreach (var el in collection)
            {
                var currentName = name + j++;
                SetSingleParameter(command, currentName, el.GetType(), el);
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
