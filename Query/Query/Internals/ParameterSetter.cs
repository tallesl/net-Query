namespace QueryLibrary
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;

    internal class ParameterSetter
    {
        private readonly bool _enumAsString;

        private readonly int? _timeout;

        internal ParameterSetter(bool enumAsString, int? timeout)
        {
            _enumAsString = enumAsString;
            _timeout = timeout;
        }

        internal DbCommand GetCommand(DbConnection connection, string sql, object parameters)
        {
            var command = connection.CreateCommand();

            command.CommandText = sql;

            if (_timeout.HasValue)
                command.CommandTimeout = _timeout.Value;

            if (parameters != null)
            {
                foreach (var kvp in ToDictionary(parameters))
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
            parameter.DbType = ToDbType(type);
            parameter.Value = (_enumAsString && value is Enum) ? value.ToString() : (value ?? DBNull.Value);

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

        // from https://github.com/tallesl/net-Dictionary
        private IDictionary<string, Tuple<Type, object>> ToDictionary(object input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (input is IDictionary<string, object>)
                return ((IDictionary<string, object>)input).ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value == null ?
                        new Tuple<Type, object>(typeof(object), kvp.Value) :
                        new Tuple<Type, object>(kvp.Value.GetType(), kvp.Value)
                );

            var dict = new Dictionary<string, Tuple<Type, object>>();

            foreach (var property in input.GetType().GetProperties())
                dict.Add(property.Name, new Tuple<Type, object>(property.PropertyType, property.GetValue(input, null)));

            foreach (var field in input.GetType().GetFields())
                dict.Add(field.Name, new Tuple<Type, object>(field.FieldType, field.GetValue(input)));

            return dict;
        }

        private static DbType ToDbType(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;

            if (type.GetElementType() == typeof(byte))
                return DbType.Binary;

            if (type == typeof(Guid))
                return DbType.Guid;

            if (type == typeof(TimeSpan))
                return DbType.Time;

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return DbType.Boolean;

                case TypeCode.Byte:
                    return DbType.Byte;

                case TypeCode.Char:
                    return DbType.StringFixedLength;

                case TypeCode.DateTime:
                    return DbType.DateTime;

                case TypeCode.Decimal:
                    return DbType.Decimal;

                case TypeCode.Double:
                    return DbType.Double;

                case TypeCode.Int16:
                    return DbType.Int16;

                case TypeCode.Int32:
                    return DbType.Int32;

                case TypeCode.Int64:
                    return DbType.Int64;

                case TypeCode.SByte:
                    return DbType.SByte;

                case TypeCode.Single:
                    return DbType.Single;

                case TypeCode.String:
                    return DbType.String;

                case TypeCode.UInt16:
                    return DbType.UInt16;

                case TypeCode.UInt32:
                    return DbType.UInt32;

                case TypeCode.UInt64:
                    return DbType.UInt64;

                default:
                    return DbType.Object;
            }
        }
    }
}