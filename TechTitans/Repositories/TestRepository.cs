using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTitans.Models;

namespace TechTitans.Repositories
{
    public class TestRepository : Repository<Test>
    {
        public Test TestMethod()
        {
            var cmd = new StringBuilder();
            cmd.Append("SELECT * FROM Test WHERE Name='Jane'");
            return _connection.Query<Test>(cmd.ToString()).FirstOrDefault();
        }
    }
}
