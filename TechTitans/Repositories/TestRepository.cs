using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTitans.Models;

namespace TechTitans.Repositories
{
    public class TestRepository : Repository<Test>
    {
        public TestRepository(IDatabaseOperations databaseOperations) : base(databaseOperations)
        {
        }

        public Test TestMethod()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM Test");
            return _databaseOperations.Query<Test>(queryBuilder.ToString()).FirstOrDefault();
        }
    }
    public class TestDemographicDetails : Repository<UserDemographicsDetails>
    {
        public TestDemographicDetails(IDatabaseOperations databaseOperations) : base(databaseOperations)
        {
        }

        public UserDemographicsDetails TestMethod()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM UserDemographicsDetails");
            return _databaseOperations.Query<UserDemographicsDetails>(queryBuilder.ToString()).FirstOrDefault();
        }
    }
    public class TestAuthorDetails : Repository<ArtistDetails>
    {
        public TestAuthorDetails(IDatabaseOperations databaseOperations) : base(databaseOperations)
        {
        }

        public ArtistDetails TestMethod()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM AuthorDetails");
            return _databaseOperations.Query<ArtistDetails>(queryBuilder.ToString()).FirstOrDefault();
        }
    }
    public class TestAdDistributionData : Repository<AdDistributionData>
    {
        public TestAdDistributionData(IDatabaseOperations databaseOperations) : base(databaseOperations)
        {
        }

        public AdDistributionData TestMethod()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM AdDistributionData");
            return _databaseOperations.Query<AdDistributionData>(queryBuilder.ToString()).FirstOrDefault();
        }
    }
    public class TestSongBasicDetails : Repository<SongDataBaseModel>
    {
        public TestSongBasicDetails(IDatabaseOperations databaseOperations) : base(databaseOperations)
        {
        }

        public SongDataBaseModel TestMethod()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM SongBasicDetails");
            return _databaseOperations.Query<SongDataBaseModel>(queryBuilder.ToString()).FirstOrDefault();
        }
    }
    public class TestUserPlaybackBehaviour : Repository<UserPlaybackBehaviour>
    {
        public TestUserPlaybackBehaviour(IDatabaseOperations databaseOperations) : base(databaseOperations)
        {
        }

        public UserPlaybackBehaviour TestMethod()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM UserPlaybackBehaviour");
            return _databaseOperations.Query<UserPlaybackBehaviour>(queryBuilder.ToString()).FirstOrDefault();
        }
    }
    public class TestSongRecommendationDetails : Repository<SongRecommendationDetails>
    {
        public TestSongRecommendationDetails(IDatabaseOperations databaseOperations) : base(databaseOperations)
        {
        }

        public SongRecommendationDetails TestMethod()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM SongRecommendationDetails");
            return _databaseOperations.Query<SongRecommendationDetails>(queryBuilder.ToString()).FirstOrDefault();
        }
    }
    public class TestSongFeatures : Repository<SongFeatures>
    {
        public TestSongFeatures(IDatabaseOperations databaseOperations) : base(databaseOperations)
        {
        }

        public SongFeatures TestMethod()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM SongFeatures");
            return _databaseOperations.Query<SongFeatures>(queryBuilder.ToString()).FirstOrDefault();
        }
    }
    public class TestTrends : Repository<Trends>
    {
        public TestTrends(IDatabaseOperations databaseOperations) : base(databaseOperations)
        {
        }

        public Trends TestMethod()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM Trends");
            return _databaseOperations.Query<Trends>(queryBuilder.ToString()).FirstOrDefault();
        }
    }
}
