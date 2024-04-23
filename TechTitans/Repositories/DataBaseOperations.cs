﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace TechTitans.Repositories
{
    public class DatabaseOperations : IDatabaseOperations
    {
        private readonly IDbConnection connection;

        public DatabaseOperations(IDbConnection connection)
        {
            this.connection = connection;
        }

        public int Execute(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.Execute(sql, param, transaction, commandTimeout, commandType);
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
        }
    }
}
