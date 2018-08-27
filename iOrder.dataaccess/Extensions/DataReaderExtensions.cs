namespace iOrder.dataaccess.Extensions
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Reflection;

    public static class DataReaderExtensions
    {
        public static T MapTo<T>(this SqlDataReader source) where T : class, new()
        {
            if (source == null || !source.HasRows) return null;

            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var result = new T();
            foreach (var property in properties)
            {
                var value = source[property.Name];
                
                property.SetValue(result, value);
            }
            return result;
        }

        public static IEnumerable<T> MapToList<T>(this SqlDataReader source) where T : class, new()
        {
            var result = new List<T>();
            while (source.Read())
            {
                var obj = source.MapTo<T>();
                result.Add(obj);
            }
            return result;
        }
    }
}
