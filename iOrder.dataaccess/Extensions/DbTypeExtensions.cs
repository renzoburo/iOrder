namespace iOrder.dataaccess.Extensions
{
    using System;
    using System.Data;

    public static class DbTypeExtensions
    {
        public static SqlDbType MapToSqlDbType(this Type type)
        {
            switch (GetTypeName(type).ToUpper())
            {
                case "DATETIME":
                    return SqlDbType.DateTime;
                case "DECIMAL":
                    return SqlDbType.Money;
                case "BOOLEAN":
                    return SqlDbType.Bit;
                case "INTEGER":
                    return SqlDbType.Int;
                case "GUID":
                    return SqlDbType.UniqueIdentifier;
                default:
                    return SqlDbType.NVarChar;
            }
        }

        private static string GetTypeName(Type type)
        {
            var nullableType = Nullable.GetUnderlyingType(type);

            var isNullableType = nullableType != null;

            return isNullableType ? nullableType.Name : type.Name;
        }
    }
}
