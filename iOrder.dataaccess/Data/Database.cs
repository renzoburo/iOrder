namespace iOrder.dataaccess.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Attributes;
    using Extensions;

    public static class Database
    {
        #region Private Properties

        static string ConnectionString => ConfigurationManager.ConnectionStrings[Constants.ConnectionStringName]?.ConnectionString ?? Constants.DefaultConnectionString;

        #endregion

        #region Public Methods

        public static IEnumerable<T> Get<T>() where T : class, new()
        {
            try
            {
                var sql = PrepareSelectStatement<T>();

                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = sql;

                        using (var reader = command.ExecuteReader())
                        {
                            var result = reader.MapToList<T>();
                            return result;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public static T GetById<T>(Guid id) where T : class, new()
        {
            if (id == Guid.Empty) return null;

            try
            {
                var sql = PrepareSelectStatement<T>(id);

                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = sql;

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var result = reader.MapTo<T>();
                                return result;
                            }

                            return null;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public static IEnumerable<T> GetProc<T>(T instance) where T : class, new()
        {
            try
            {
                var parameters = PrepareSelectParameters(instance, true);

                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = $"[dbo].[uspGet{typeof(T).Name}]";

                        if (parameters != null && parameters.Any())
                            command.Parameters.AddRange(parameters);

                        using (var reader = command.ExecuteReader())
                        {
                            var result = reader.MapToList<T>();
                            return result;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public static T GetByIdProc<T>(T instance) where T : class, new()
        {
            try
            {
                var parameters = PrepareSelectParameters(instance, true);

                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = $"[dbo].[uspGet{typeof(T).Name}]";

                        if (parameters != null && parameters.Any())
                            command.Parameters.AddRange(parameters);

                        using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            var result = reader.MapTo<T>();
                            return result;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public static T Save<T>(T instance) where T : class, new()
        {
            try
            {
                var fields = GetFields<T>();
                var primaryKey = fields.FirstOrDefault(f => f.GetCustomAttributes(typeof(PrimaryKeyAttribute)).Any());

                if (primaryKey == null)
                    throw new ArgumentException($"Primary key not defined on class '{typeof(T).Name}'");

                var value = primaryKey.GetValue(instance);

                var primaryKeyValue = value != null ? (Guid)primaryKey.GetValue(instance) : (Guid?)null;

                if (primaryKeyValue == null) return Create(instance);

                return Update(instance);
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public static T SaveProc<T>(T instance) where T : class, new()
        {
            try
            {
                var fields = GetFields<T>();
                var primaryKey = fields.FirstOrDefault(f => f.GetCustomAttributes(typeof(PrimaryKeyAttribute)).Any());

                if (primaryKey == null)
                    throw new ArgumentException($"Primary key not defined on class '{typeof(T).Name}'");

                var value = primaryKey.GetValue(instance);

                var primaryKeyValue = value != null ? (Guid)primaryKey.GetValue(instance) : (Guid?)null;

                if (primaryKeyValue == null) return CreateProc(instance);

                return UpdateProc(instance);
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public static bool Delete<T>(T instance)
        {
            try
            {
                var sql = PrepareDeleteStatement<T>();
                var parameters = PrepareDeleteParameters(instance);

                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = sql;
                        if (parameters != null && parameters.Any())
                            command.Parameters.AddRange(parameters);

                        var rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected == 1;
                    }
                }
            }
            catch (Exception exception)
            {
                return false;
            }
        }

        public static bool DeleteProc<T>(T instance)
        {
            try
            {
                var parameters = PrepareDeleteParameters(instance, true);

                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = $"[dbo].[uspDelete{typeof(T).Name}]";
                        if (parameters != null && parameters.Any())
                            command.Parameters.AddRange(parameters);

                        var rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected == 1;
                    }
                }
            }
            catch (Exception exception)
            {
                return false;
            }
        }

        #endregion

        #region Private Methods

        static T Create<T>(T instance) where T : class, new()
        {
            try
            {
                var sql = PrepareInsertStatement<T>();
                var parameters = PrepareInsertParameters(instance);

                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = sql;
                        if (parameters != null && parameters.Any())
                            command.Parameters.AddRange(parameters);

                        var idValue = command.ExecuteScalar();

                        if (idValue != null)
                        {
                            var id = new Guid(idValue.ToString());
                            return GetById<T>(id);
                        }

                        return null;
                    }
                }
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        static T CreateProc<T>(T instance) where T : class, new()
        {
            try
            {
                var parameters = PrepareInsertProcParameters(instance, true);

                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = $"[dbo].[uspSave{typeof(T).Name}]";
                        if (parameters != null && parameters.Any())
                            command.Parameters.AddRange(parameters);

                        var rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 1)
                        {
                            var idValue = GetInsertRowId<T>(command);
                            return GetById<T>(idValue);
                        }

                        return null;
                    }
                }
            }
            catch (Exception exception)
            {
                return null;

            }
        }

        static T UpdateProc<T>(T instance) where T : class, new()
        {
            try
            {
                var parameters = PrepareUpdateParameters(instance, true);

                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = $"[dbo].[uspSave{typeof(T).Name}]";
                        if (parameters != null && parameters.Any())
                            command.Parameters.AddRange(parameters);

                        var rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 1)
                        {
                            var idValue = GetInsertRowId<T>(command);
                            return GetById<T>(idValue);
                        }
                        return null;
                    }
                }
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        static T Update<T>(T instance) where T : class, new()
        {
            try
            {
                var sql = PrepareUpdateStatement<T>();
                var parameters = PrepareUpdateParameters(instance);

                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = sql;
                        if (parameters != null && parameters.Any())
                            command.Parameters.AddRange(parameters);

                        var rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 1)
                        {
                            var idValue = GetInsertRowId<T>(command);
                            return GetById<T>(idValue);
                        }
                        return null;
                    }
                }
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        static string PrepareSelectStatement<T>()
        {
            var table = (TableAttribute)typeof(T).GetCustomAttributes(typeof(TableAttribute)).FirstOrDefault();
            if (table is null)
                throw new ArgumentException($"Table attribute not defined on class '{typeof(T).Name}'.");

            var fields = GetFields<T>();
            var fieldNames = from field in fields select field.Name;
            var sql = $"SELECT {fieldNames.Join(",")} FROM  [{table.Schema}].[{table.Name}]";

            return sql;
        }

        static string PrepareSelectStatement<T>(Guid id)
        {
            var table = (TableAttribute)typeof(T).GetCustomAttributes(typeof(TableAttribute)).FirstOrDefault();
            if (table is null)
                throw new ArgumentException($"Table attribute not defined on class '{typeof(T).Name}'.");

            var fields = GetFields<T>();
            var primaryKey = fields.First(f => f.GetCustomAttributes(typeof(PrimaryKeyAttribute), true).Any());
            var fieldNames = from field in fields select field.Name;
            var sql = $"SELECT {fieldNames.Join(",")} FROM  [{table.Schema}].[{table.Name}] WHERE {primaryKey.Name} = '{id.ToString()}'";

            return sql;
        }

        static string PrepareInsertStatement <T>()
        {
            var table = (TableAttribute)typeof(T).GetCustomAttributes(typeof(TableAttribute)).FirstOrDefault();
            if (table is null)
                throw new ArgumentException($"Table attribute not defined on class '{typeof(T).Name}'.");

            var fields = GetFields<T>();
            var fieldNames = GetFieldsNames(fields);
            var primaryKey = fields.First(f => f.GetCustomAttributes(typeof(PrimaryKeyAttribute), true).Any());
            var enumerable = fieldNames as string[] ?? fieldNames.ToArray(); //To avoid the "multiple ienumerable usage" warning.
            var sql = $"INSERT [{table.Schema}].[{table.Name}] ({enumerable.Join(",")}) OUTPUT INSERTED.{primaryKey.Name} VALUES (@{enumerable.Join(",@")})";
            return sql;
        }

        static string PrepareUpdateStatement<T>()
        {
            var table = (TableAttribute)typeof(T).GetCustomAttributes(typeof(TableAttribute)).FirstOrDefault();
            if (table is null)
                throw new ArgumentException($"Table attribute not defined on class '{typeof(T).Name}'.");

            var fields = GetFields<T>();
            var fieldList = fields as PropertyInfo[] ?? fields.ToArray(); //To avoid the "multiple ienumerable usage" warning.
            var primaryKey = fieldList.First(f => f.GetCustomAttributes(typeof(PrimaryKeyAttribute), true).Any());
            var fieldNames = GetFieldsNames(fieldList);
            var enumerable = fieldNames as string[] ?? fieldNames.ToArray(); //To avoid the "multiple ienumerable usage" warning.
            var stringBuilder = new StringBuilder();

            stringBuilder.Clear();
            stringBuilder.Append($"UPDATE [{table.Schema}].[{table.Name}] SET");

            var setFields = new List<string>();
            foreach (var fieldName in enumerable)
            {
                setFields.Add($" [{fieldName}] = @{fieldName}");
            }

            stringBuilder.Append(setFields.Join(","));
            stringBuilder.Append($" WHERE [{primaryKey.Name}] = @{primaryKey.Name}");

            return stringBuilder.ToString();
        }

        static string PrepareDeleteStatement<T>()
        {
            var table = (TableAttribute)typeof(T).GetCustomAttributes(typeof(TableAttribute)).FirstOrDefault();
            if (table is null)
                throw new ArgumentException($"Table attribute not defined on class '{typeof(T).Name}'.");

            var primaryKey = GetFields<T>().First(f => f.GetCustomAttributes(typeof(PrimaryKeyAttribute), true).Any());
            var stringBuilder = new StringBuilder();

            stringBuilder.Clear();
            stringBuilder.Append($"DELETE [{table.Schema}].[{table.Name}]");
            stringBuilder.Append($" WHERE [{primaryKey.Name}] = @{primaryKey.Name}");

            return stringBuilder.ToString();
        }

        private static SqlParameter[] PrepareSelectParameters<T>(T instance, bool proc = false)
        {
            var fields = GetFields<T>();
            var primaryKey = fields.FirstOrDefault(f => f.GetCustomAttributes(typeof(PrimaryKeyAttribute)).Any());
            return new[] { CreateParameter(primaryKey, instance, proc) };
        }

        static SqlParameter[] PrepareInsertProcParameters <T>(T instance, bool proc = false)
        {
            var fields = GetFields<T>();
            var result = fields.Select(s => CreateParameter(s, instance, proc)).ToList();
            return result.Any() ? result.ToArray() : null;
        }

        static SqlParameter[] PrepareInsertParameters<T>(T instance, bool proc = false)
        {
            var fields = GetFields<T>().Where(w => !w.GetCustomAttributes(typeof(PrimaryKeyAttribute)).Any());
            var result = fields.Select(s => CreateParameter(s, instance, proc)).ToList();
            return result.Any() ? result.ToArray() : null;
        }

        private static SqlParameter[] PrepareUpdateParameters<T>(T instance, bool proc = false)
        {
            var fields = GetFields<T>();
            var result = fields.Select(field => CreateParameter(field, instance, proc)).ToList();
            return result.Any() ? result.ToArray() : null;
        }

        private static SqlParameter[] PrepareDeleteParameters<T>(T instance, bool proc = false)
        {
            var fields = GetFields<T>();
            var primaryKey = fields.FirstOrDefault(f => f.GetCustomAttributes(typeof(PrimaryKeyAttribute)).Any());
            return new[] { CreateParameter(primaryKey, instance, proc) };
        }

        static SqlParameter CreateParameter(PropertyInfo field, object instance, bool proc = false)
        {
            return new SqlParameter
            {
                ParameterName = $"@{field.Name}",
                SqlDbType = field.PropertyType.MapToSqlDbType(),
                Direction = GetParameterDirection(field),
                Value = field.GetValue(instance)
            };
        }

        static Guid GetInsertRowId<T>(SqlCommand command)
        {
            var fields = GetFields<T>();
            var primaryKey = fields.FirstOrDefault(f => f.GetCustomAttributes(typeof(PrimaryKeyAttribute)).Any());

            if (primaryKey == null)
                throw new ArgumentException($"Primary key not define for table '{typeof(T).Name}'");

            return new Guid(command.Parameters[$"@{primaryKey.Name}"].Value.ToString());
        }

        static ParameterDirection GetParameterDirection(PropertyInfo field)
        {
            var directionAttr = (DirectionAttribute)field.GetCustomAttributes(typeof(DirectionAttribute)).FirstOrDefault();

            if (directionAttr == null)
                throw new ArgumentException($"'Direction' attribute not defined on '{field.Name}' field");

            if (directionAttr.DirectionType == DirectionType.Output || directionAttr.DirectionType == DirectionType.InputOutput)
                return ParameterDirection.InputOutput;

            return ParameterDirection.Input;
        }

        static IEnumerable<PropertyInfo> GetFields<T>()
        {
            return typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }

        static IEnumerable<string> GetFieldsNames(IEnumerable<PropertyInfo> fields)
        {
            var result = from field in fields where !field.GetCustomAttributes(typeof(PrimaryKeyAttribute)).Any() select field.Name;
            return result;
        }

        #endregion
    }
}
