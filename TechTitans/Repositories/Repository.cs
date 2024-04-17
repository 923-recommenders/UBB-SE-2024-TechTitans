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

namespace TechTitans.Repositories
{
    
    public class Repository<T> : IRepository<T> where T : class
    {
        public IDbConnection _connection;
        private readonly IConfiguration _configuration = MauiProgram.Configuration;

        public Repository()
        {

            _connection = new Microsoft.Data.SqlClient.SqlConnection(_configuration.GetConnectionString("TechTitansDev"));
        }

        public bool Add(T entity)
        {
            int rowsAffectedByQueryExecution = 0;
            try
            {
                string tableName = GetTableName();
                string columns = GetColumns(excludeKey: true);
                string properties = GetPropertyNames(excludeKey: true);
                string query = $"INSERT INTO {tableName} ({columns}) VALUES ({properties})";

                rowsAffectedByQueryExecution = _connection.Execute(query, entity);
            }
            catch (Exception ex) { }

            return rowsAffectedByQueryExecution > 0 ? true : false;
        }

        public bool Delete(T entity)
        {
            int rowsAffectedByQueryExecution = 0;
            try
            {
                string tableName = GetTableName();
                string keyColumn = GetKeyColumnName();
                string keyProperty = GetKeyPropertyName();
                string query = $"DELETE FROM {tableName} WHERE {keyColumn} = @{keyProperty}";

                rowsAffectedByQueryExecution = _connection.Execute(query, entity);
            }
            catch (Exception ex) { }

            return rowsAffectedByQueryExecution > 0 ? true : false;
        }

        public IEnumerable<T> GetAll()
        {
            IEnumerable<T> result = null;
            try
            {
                string tableName = GetTableName();
                string query = $"SELECT * FROM {tableName}";

                result = _connection.Query<T>(query);
            }
            catch (Exception ex) { }

            return result;
        }

        public T GetById(int Id)
        {
            IEnumerable<T> resultOfQueryExecution = null;
            try
            {
                string tableName = GetTableName();
                string keyColumn = GetKeyColumnName();
                string query = $"SELECT * FROM {tableName} WHERE {keyColumn} = '{Id}'";

                resultOfQueryExecution = _connection.Query<T>(query);
            }
            catch (Exception ex) { }

            return resultOfQueryExecution.FirstOrDefault();
        }

        public bool Update(T entity)
        {
            int rowsAffectedByQueryExecution = 0;
            try
            {
                string tableName = GetTableName();
                string keyColumn = GetKeyColumnName();
                string keyProperty = GetKeyPropertyName();

                StringBuilder query = new StringBuilder();
                query.Append($"UPDATE {tableName} SET ");

                foreach (var property in GetProperties(true))
                {
                    var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();

                    string propertyName = property.Name;
                    string columnName = columnAttribute.Name;

                    query.Append($"{columnName} = @{propertyName},");
                }

                query.Remove(query.Length - 1, 1);

                query.Append($" WHERE {keyColumn} = @{keyProperty}");

                rowsAffectedByQueryExecution = _connection.Execute(query.ToString(), entity);
            }
            catch (Exception ex) { }

            return rowsAffectedByQueryExecution > 0 ? true : false;
        }

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

        protected IEnumerable<PropertyInfo> GetProperties(bool excludeKey = false)
        {
            var properties = typeof(T).GetProperties()
                .Where(propertyInformation => !excludeKey || propertyInformation.GetCustomAttribute<KeyAttribute>() == null);

            return properties;
        }

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
