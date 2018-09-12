namespace QueryLibrary
{
    using System;
    using System.Data;

    internal static class DbTypeConverter
    {
        internal static DbType ToDbType(this Type type)
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