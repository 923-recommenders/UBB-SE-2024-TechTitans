using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TechTitans.Models;

namespace TechTitans.Repositories
{
    internal class RepositoryContext
    {
        private static IDatabaseOperations databaseOperations = new DatabaseOperations();
        private static Repository<UserPlaybackBehaviour> userSongBehaviourRepository;
        private static Repository<AdDistributionData> adDistributionDataRepository;
        private static Repository<ArtistDetails> artistDetailsRepository;
        private static Repository<SongRecommendationDetails> songRecommendationDetailsRepository;
        private static Repository<SongDataBaseModel> songDataBaseModelRepository;
        private static Repository<SongFeatures> songFeaturesRepository;
        private static SongBasicDetailsRepository songBasicDetailsRepository;
        private static UserPlaybackBehaviourRepository userPlaybackBehaviourRepository;
        private static UserSongRepository userSongRepository;

        public static Repository<UserPlaybackBehaviour> UserSongBehaviourRepositoryInstance
        {
            get
            {
                if (userSongBehaviourRepository == null)
                {
                    userSongBehaviourRepository = new Repository<UserPlaybackBehaviour>(databaseOperations);
                }
                return userSongBehaviourRepository;
            }
        }

        public static Repository<AdDistributionData> AdDistributionDataRepositoryInstance
        {
            get
            {
                if (adDistributionDataRepository == null)
                {
                    adDistributionDataRepository = new Repository<AdDistributionData>(databaseOperations);
                }
                return adDistributionDataRepository;
            }
        }

        public static Repository<ArtistDetails> ArtistDetailsRepositoryInstance
        {
            get
            {
                if (artistDetailsRepository == null)
                {
                    artistDetailsRepository = new Repository<ArtistDetails>(databaseOperations);
                }
                return artistDetailsRepository;
            }
        }

        public static Repository<SongRecommendationDetails> SongRecommendationDetailsRepositoryInstance
        {
            get
            {
                if (songRecommendationDetailsRepository == null)
                {
                    songRecommendationDetailsRepository = new Repository<SongRecommendationDetails>(databaseOperations);
                }
                return songRecommendationDetailsRepository;
            }
        }

        public static Repository<SongDataBaseModel> SongDataBaseModelRepositoryInstance
        {
            get
            {
                if (songDataBaseModelRepository == null)
                {
                    songDataBaseModelRepository = new Repository<SongDataBaseModel>(databaseOperations);
                }
                return songDataBaseModelRepository;
            }
        }

        public static Repository<SongFeatures> SongFeaturesRepositoryInstance
        {
            get
            {
                if (songFeaturesRepository == null)
                {
                    songFeaturesRepository = new Repository<SongFeatures>(databaseOperations);
                }
                return songFeaturesRepository;
            }
        }

        public static SongBasicDetailsRepository SongBasicDetailsRepositoryInstance
        {
            get
            {
                if (songBasicDetailsRepository == null)
                {
                    songBasicDetailsRepository = new SongBasicDetailsRepository(databaseOperations);
                }
                return songBasicDetailsRepository;
            }
        }

        public static UserPlaybackBehaviourRepository UserPlaybackBehaviourRepositoryInstance
        {
            get
            {
                if (userPlaybackBehaviourRepository == null)
                {
                    userPlaybackBehaviourRepository = new UserPlaybackBehaviourRepository(databaseOperations);
                }
                return userPlaybackBehaviourRepository;
            }
        }

        public static UserSongRepository UserSongRepositoryInstance
        {
            get
            {
                if (userSongRepository == null)
                {
                    userSongRepository = new UserSongRepository(databaseOperations);
                }
                return userSongRepository;
            }
        }
    }
}
