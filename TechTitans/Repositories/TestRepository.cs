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
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM Test");
            return _connection.Query<Test>(queryBuilder.ToString()).FirstOrDefault();
        }
    }
    public class TestDemographicDetails : Repository<UserDemographicsDetails>
    {
        public UserDemographicsDetails TestMethod()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM UserDemographicsDetails");
            return _connection.Query<UserDemographicsDetails>(queryBuilder.ToString()).FirstOrDefault();
        }
    }
    public class TestAuthorDetails : Repository<AuthorDetails>
    {
        public AuthorDetails TestMethod()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM AuthorDetails");
            return _connection.Query<AuthorDetails>(queryBuilder.ToString()).FirstOrDefault();
        }
    }
    public class TestAdDistributionData : Repository<AdDistributionData>
    {
        public AdDistributionData TestMethod()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM AdDistributionData");
            return _connection.Query<AdDistributionData>(queryBuilder.ToString()).FirstOrDefault();
        }
    }
    public class TestSongBasicDetails : Repository<SongDataBaseModel>
    {
        public SongDataBaseModel TestMethod()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM SongBasicDetails");
            return _connection.Query<SongDataBaseModel>(queryBuilder.ToString()).FirstOrDefault();
        }
    }
    public class TestUserPlaybackBehaviour : Repository<UserPlaybackBehaviour>
    {
        public UserPlaybackBehaviour TestMethod()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM UserPlaybackBehaviour");
            return _connection.Query<UserPlaybackBehaviour>(queryBuilder.ToString()).FirstOrDefault();
        }
    }
    public class TestSongRecommendationDetails : Repository<SongRecommendationDetails>
    {
        public SongRecommendationDetails TestMethod()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM SongRecommendationDetails");
            return _connection.Query<SongRecommendationDetails>(queryBuilder.ToString()).FirstOrDefault();
        }
    }
    public class TestSongFeatures : Repository<SongFeatures>
    {
        public SongFeatures TestMethod()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM SongFeatures");
            return _connection.Query<SongFeatures>(queryBuilder.ToString()).FirstOrDefault();
        }
    }
    public class TestTrends : Repository<Trends>
    {
        public Trends TestMethod()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM Trends");
            return _connection.Query<Trends>(queryBuilder.ToString()).FirstOrDefault();
        }
    }
}
