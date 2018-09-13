namespace QueryLibrary
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    internal class ParameterSetter
    {
        private readonly QueryOptions _options;

        internal ParameterSetter(QueryOptions options) => _options = options;

        internal DbCommand GetCommand(DbConnection connection, string sql, object parameters)
        {
            var command = connection.CreateCommand();

            command.CommandText = sql;

            if (_options.CommandTimeout.HasValue)
                command.CommandTimeout = _options.CommandTimeout.Value;

            if (parameters != null)
            {
                foreach (var kvp in DictionaryMaker.MakeWithType(parameters))
                {
                    var name = kvp.Key;
                    var type = kvp.Value.Item1;
                    var value = kvp.Value.Item2;
                    var enumerable = value as IEnumerable;

                    if (enumerable == null || value is string)
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

            command.Parameters.Add(parameter);
        }

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

            var clause = string.Join(",", parameters);
            command.CommandText = command.CommandText.Replace("@" + name, clause);
        }
    }
}