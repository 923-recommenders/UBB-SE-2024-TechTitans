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
    public class TestRepositoryDemographic : Repository<UserDemographicsDetails>
    {
        public UserDemographicsDetails TestMethod()
        {
            var cmd = new StringBuilder();
            cmd.Append("SELECT * FROM UserDemographicsDetails");
            // return _connection.Query<Test>(cmd.ToString()).FirstOrDefault();
            return _connection.Query<UserDemographicsDetails>(cmd.ToString()).FirstOrDefault();
        }
    }
}
