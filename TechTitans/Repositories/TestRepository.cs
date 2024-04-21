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
        public TestRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public Test TestMethod()
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                connection.Open();
                var queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT * FROM Test");
                return connection.Query<Test>(queryBuilder.ToString()).FirstOrDefault();
            }
        }
    }

    public class TestDemographicDetails : Repository<UserDemographicsDetails>
    {
        public TestDemographicDetails(IConfiguration configuration) : base(configuration)
        {
        }

        public UserDemographicsDetails TestMethod()
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                connection.Open();
                var queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT * FROM UserDemographicsDetails");
                return connection.QueryFirstOrDefault<UserDemographicsDetails>(queryBuilder.ToString());
            }
        }
    }
    public class TestAuthorDetails : Repository<ArtistDetails>
    {
        public TestAuthorDetails(IConfiguration configuration) : base(configuration)
        {
        }
        public ArtistDetails TestMethod()
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                connection.Open();
                var queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT * FROM AuthorDetails");
                return connection.Query<ArtistDetails>(queryBuilder.ToString()).FirstOrDefault();
            }
        }
    }
    public class TestAdDistributionData : Repository<AdDistributionData>
    {
        public TestAdDistributionData(IConfiguration configuration) : base(configuration)
        {
        }

        public AdDistributionData TestMethod()
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                connection.Open();
                var queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT * FROM AdDistributionData");
                return connection.Query<AdDistributionData>(queryBuilder.ToString()).FirstOrDefault();
            }
        }
    }
    public class TestSongBasicDetails : Repository<SongDataBaseModel>
    {
        public TestSongBasicDetails(IConfiguration configuration) : base(configuration)
        {
        }
        public SongDataBaseModel TestMethod()
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                connection.Open();
                var queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT * FROM SongBasicDetails");
                return connection.Query<SongDataBaseModel>(queryBuilder.ToString()).FirstOrDefault();
            }
        }
    }
    public class TestUserPlaybackBehaviour : Repository<UserPlaybackBehaviour>
    {
        public TestUserPlaybackBehaviour(IConfiguration configuration) : base(configuration)
        {
        }
        public UserPlaybackBehaviour TestMethod()
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                connection.Open();
                var queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT * FROM UserPlaybackBehaviour");
                return connection.Query<UserPlaybackBehaviour>(queryBuilder.ToString()).FirstOrDefault();
            }
        }
    }
    public class TestSongRecommendationDetails : Repository<SongRecommendationDetails>
    {
        public TestSongRecommendationDetails(IConfiguration configuration) : base(configuration)
        {
        }
        public SongRecommendationDetails TestMethod()
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                connection.Open();
                var queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT * FROM SongRecommendationDetails");
                return connection.Query<SongRecommendationDetails>(queryBuilder.ToString()).FirstOrDefault();
            }
        }
    }
    public class TestSongFeatures : Repository<SongFeatures>
    {
        public TestSongFeatures(IConfiguration configuration) : base(configuration)
        {
        }
        public SongFeatures TestMethod()
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                connection.Open();
                var queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT * FROM SongFeatures");
                return connection.Query<SongFeatures>(queryBuilder.ToString()).FirstOrDefault();
            }
        }
    }
    public class TestTrends : Repository<Trends>
    {
        public TestTrends(IConfiguration configuration) : base(configuration)
        {
        }
        public Trends TestMethod()
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                connection.Open();
                var queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT * FROM Trends");
                return connection.Query<Trends>(queryBuilder.ToString()).FirstOrDefault();
            }
        }
    }
}
