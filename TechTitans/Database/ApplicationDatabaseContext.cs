using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;



namespace TechTitans.Database
{
    public class ApplicationDatabaseContext
    {
        private readonly string _connectionString;

        public ApplicationDatabaseContext(IConfiguration configuration)
        {
            var dbIp = configuration.GetSection("Database:IP").Value;
            var dbUser = configuration.GetSection("Database:User").Value;
            var dbPass = configuration.GetSection("Database:Password").Value;
            var dbSchema = configuration.GetSection("Database:Schema").Value;

            _connectionString = $"Data Source=tcp:{dbIp},1433;User ID={dbUser};" +
                                $"Password={dbPass};Initial Catalog={dbSchema};TrustServerCertificate=True";
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public string getConnectionString() { return _connectionString; }
    }
}
