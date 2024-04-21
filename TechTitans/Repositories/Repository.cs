using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using static Dapper.SqlMapper;
using Microsoft.Extensions.Configuration;
using TechTitans.Database;
using System.Runtime.CompilerServices;

namespace TechTitans.Repositories
{
    
    public class Repository<T> : IRepository<T> where T : class
    {
        public IDbConnection _connection;
        public readonly ApplicationDatabaseContext _databaseHelper;
        private readonly IConfiguration _configuration = MauiProgram.Configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        public Repository(IConfiguration configuration)
        {
            //_databaseHelper = databaseHelper;
            _databaseHelper = new ApplicationDatabaseContext(configuration);
            _connection = new Microsoft.Data.SqlClient.SqlConnection(_databaseHelper.getConnectionString());
        }


        /// <summary>
        /// Adds a new entity to the database.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>True if the entity was added successfully; otherwise, false.</returns>
        public bool Add(T entity)
        {
            int rowsAffectedByQueryExecution = 0;
            try
            {
                using (var connection = _databaseHelper.GetConnection())
                {
                    connection.Open();
                    string tableName = GetTableName();
                    string columns = GetColumns(excludeKey: true);
                    string properties = GetPropertyNames(excludeKey: true);

                    string query = $"INSERT INTO {tableName} ({columns}) VALUES ({properties})";

                    using (var command = new SqlCommand(query, connection))
                    {
                        // Set parameters for each property
                        foreach (var property in GetProperties(excludeKey: true))
                        {
                            var parameter = new SqlParameter($"@{property.Name}", property.GetValue(entity) ?? DBNull.Value);
                            command.Parameters.Add(parameter);
                        }

                        rowsAffectedByQueryExecution = command.ExecuteNonQuery();
                    }
                }
             }
            catch (Exception ex) { }

            return rowsAffectedByQueryExecution > 0 ? true : false;
        }

        /// <summary>
        /// Deletes an entity from the database.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>True if the entity was deleted successfully; otherwise, false.</returns>
        public bool Delete(T entity)
        {
            int rowsAffectedByQueryExecution = 0;
            try
            {
                using (var connection = _databaseHelper.GetConnection())
                {
                    connection.Open();
                    string tableName = GetTableName();
                    string keyColumn = GetKeyColumnName();
                    string keyProperty = GetKeyPropertyName();

                    string query = $"DELETE FROM {tableName} WHERE {keyColumn} = @{keyProperty}";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue($"@{keyProperty}", entity.GetType().GetProperty(keyProperty).GetValue(entity));
                        rowsAffectedByQueryExecution = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) { }

            return rowsAffectedByQueryExecution > 0 ? true : false;
        }

        /// <summary>
        /// Retrieves all entities from the database.
        /// </summary>
        /// <returns>An enumerable collection of entities.</returns>
        public IEnumerable<T> GetAll()
        {
            IEnumerable<T> result = null;
            try
            {
                using (var connection = _databaseHelper.GetConnection())
                {
                    //Console.WriteLine($"Connection String: {connection.ConnectionString}");
                    // Log the state of the connection
                    //Console.WriteLine($"Connection State: {connection.State}");
                    // Log the database name
                    //Console.WriteLine($"Database: {connection.Database}");
                    connection.Open();
                    string tableName = GetTableName();
                    string query = $"SELECT * FROM {tableName}";

                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                var entities = new List<T>();
                                while (reader.Read())
                                {
                                    var entity = Activator.CreateInstance<T>();
                                    foreach (var property in GetProperties())
                                    {
                                        var columnName = property.GetCustomAttribute<ColumnAttribute>()?.Name ?? property.Name;
                                        var value = reader[columnName];
                                        if (value != DBNull.Value)
                                        {
                                            property.SetValue(entity, value);
                                        }
                                    }
                                    entities.Add(entity);
                                }
                                result = entities;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }

            return result;
        }

        /// <summary>
        /// Retrieves an entity by its ID.
        /// </summary>
        /// <param name="Id">The ID of the entity.</param>
        /// <returns>The entity if found; otherwise, null.</returns>

        public T GetById(int Id)
        {
            IEnumerable<T> resultOfQueryExecution = null;
            try
            {
                using (var connection = _databaseHelper.GetConnection())
                {
                    connection.Open();
                    string tableName = GetTableName();
                    string keyColumn = GetKeyColumnName();
                    string query = $"SELECT * FROM {tableName} WHERE {keyColumn} = '{Id}'";

                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                var entities = new List<T>();
                                while (reader.Read())
                                {
                                    var entity = Activator.CreateInstance<T>();
                                    foreach (var property in GetProperties())
                                    {
                                        var columnName = property.GetCustomAttribute<ColumnAttribute>()?.Name ?? property.Name;
                                        var value = reader[columnName];
                                        if (value != DBNull.Value)
                                        {
                                            property.SetValue(entity, value);
                                        }
                                    }
                                    entities.Add(entity);
                                }
                                resultOfQueryExecution = entities;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }

            return resultOfQueryExecution.FirstOrDefault();
        }

        /// <summary>
        /// Updates an existing entity in the database.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>True if the entity was updated successfully; otherwise, false.</returns>
        public bool Update(T entity)
        {
            int rowsAffectedByQueryExecution = 0;
            try
            {
                using (var connection = _databaseHelper.GetConnection())
                {
                    connection.Open();
                    string tableName = GetTableName();
                    string keyColumn = GetKeyColumnName();
                    string keyProperty = GetKeyPropertyName();

                    var properties = GetProperties(excludeKey: true);

                    var updateColumns = properties.Select(property =>
                    {
                        var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
                        var columnName = columnAttribute != null ? columnAttribute.Name : property.Name;
                        return $"{columnName} = @{property.Name}";
                    });

                    string updateColumnsString = string.Join(", ", updateColumns);

                    string query = $"UPDATE {tableName} SET {updateColumnsString} WHERE {keyColumn} = @{keyProperty}";

                    using (var command = new SqlCommand(query, connection))
                    {
                        // Set parameters for each property
                        foreach (var property in properties)
                        {
                            var parameter = new SqlParameter($"@{property.Name}", property.GetValue(entity) ?? DBNull.Value);
                            command.Parameters.Add(parameter);
                        }

                        // Set parameter for the key property
                        var keyPropertyValue = entity.GetType().GetProperty(keyProperty).GetValue(entity) ?? DBNull.Value;
                        var keyParameter = new SqlParameter($"@{keyProperty}", keyPropertyValue);
                        command.Parameters.Add(keyParameter);

                        rowsAffectedByQueryExecution = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) { }

            return rowsAffectedByQueryExecution > 0 ? true : false;
        }

        /// <summary>
        /// Retrieves the name of the table associated with the entity type T.
        /// </summary>
        /// <returns>The name of the table.</returns>
        private string GetTableName()
        {
            string tableName = "";
            var type = typeof(T);
            var tableAttribute = type.GetCustomAttribute<TableAttribute>();
            if (tableAttribute != null)
            {
                tableName = tableAttribute.Name;
                return tableName;
            }

            return type.Name + "s";
        }

        /// <summary>
        /// Retrieves the name of the key column for the entity type T.
        /// </summary>
        /// <returns>The name of the key column.</returns>
        public static string GetKeyColumnName()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object[] keyAttributes = property.GetCustomAttributes(typeof(KeyAttribute), true);

                if (keyAttributes != null && keyAttributes.Length > 0)
                {
                    object[] columnAttributes = property.GetCustomAttributes(typeof(ColumnAttribute), true);

                    if (columnAttributes != null && columnAttributes.Length > 0)
                    {
                        ColumnAttribute columnAttribute = (ColumnAttribute)columnAttributes[0];
                        return columnAttribute.Name;
                    }
                    else
                    {
                        return property.Name;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Retrieves a comma-separated list of column names for the entity type T, 
        /// excluding the key column if specified.
        /// </summary>
        /// <param name="excludeKey">Indicates whether to exclude the key column
        /// from the list.</param>
        /// <returns>A comma-separated list of column names.</returns>
        private string GetColumns(bool excludeKey = false)
        {
            var type = typeof(T);
            var columns = string.Join(", ", type.GetProperties()
                .Where(propertyInformation => !excludeKey || !propertyInformation.IsDefined(typeof(KeyAttribute)))
                .Select(propertyInformation =>
                {
                    var columnAttribute = propertyInformation.GetCustomAttribute<ColumnAttribute>();
                    return columnAttribute != null ? columnAttribute.Name : propertyInformation.Name;
                }));

            return columns;
        }

        /// <summary>
        /// Retrieves a comma-separated list of property names for 
        /// the entity type T, excluding the key property if specified.
        /// </summary>
        /// <param>
        /// name="excludeKey">Indicates whether to 
        /// exclude the key property from the list.
        /// </param>
        /// <returns>A comma-separated list of property names.</returns>
        protected string GetPropertyNames(bool excludeKey = false)
        {
            var properties = typeof(T).GetProperties()
                .Where(propertyInformation => !excludeKey || propertyInformation.GetCustomAttribute<KeyAttribute>() == null);

            var values = string.Join(", ", properties.Select(propertyInformation =>
            {
                return $"@{propertyInformation.Name}";
            }));

            return values;
        }

        /// <summary>
        /// Retrieves a collection of PropertyInfo objects for the entity type T,
        /// excluding the key property if specified.
        /// </summary>
        /// <param name="excludeKey">Indicates whether to exclude the 
        /// key property from the collection.</param>
        /// <returns>A collection of PropertyInfo objects.</returns>
        protected IEnumerable<PropertyInfo> GetProperties(bool excludeKey = false)
        {
            var properties = typeof(T).GetProperties()
                .Where(propertyInformation => !excludeKey || propertyInformation.GetCustomAttribute<KeyAttribute>() == null);

            return properties;
        }

        /// <summary>
        /// Retrieves the name of the key property for the entity type T.
        /// </summary>
        /// <returns>The name of the key property.</returns>
        protected string GetKeyPropertyName()
        {
            var properties = typeof(T).GetProperties()
                .Where(propertyInformation => propertyInformation.GetCustomAttribute<KeyAttribute>() != null);

            if (properties.Any())
            {
                return properties.FirstOrDefault().Name;
            }

            return null;
        }


    }
}
