using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTitans.Repositories;

namespace TechTitans.Services
{
    internal class ServiceContext
    {
        private static ArtistSongDashboardController artistSongDashboardController;
        private static FullDetailsOnSongController fullDetailsOnSongController;
        private static RecapController recapController;
        private static TopGenresController topGenresController;
        private static UserController userController;

        public static ArtistSongDashboardController ArtistSongDashboardControllerInstance
        {
            get
            {
                if (artistSongDashboardController == null)
                {
                    var songRepository = RepositoryContext.SongDataBaseModelRepositoryInstance;
                    var featureRepository = RepositoryContext.SongFeaturesRepositoryInstance;
                    var songRecommendationRepository = RepositoryContext.SongRecommendationDetailsRepositoryInstance;
                    var artistRepository = RepositoryContext.ArtistDetailsRepositoryInstance;
                    artistSongDashboardController = new ArtistSongDashboardController(songRepository, featureRepository, songRecommendationRepository, artistRepository);
                }
                return artistSongDashboardController;
            }
        }

        public static FullDetailsOnSongController FullDetailsOnSongController
        {
            get
            {
                if (fullDetailsOnSongController == null)
                {
                    var userPlaybackBehaviourRepository = RepositoryContext.UserSongBehaviourRepositoryInstance;
                    var adDistributionRepository = RepositoryContext.AdDistributionDataRepositoryInstance;
                    fullDetailsOnSongController = new FullDetailsOnSongController(userPlaybackBehaviourRepository, adDistributionRepository);
                }
                return fullDetailsOnSongController;
            }
        }

        public static RecapController RecapControllerInstance
        {
            get
            {
                if (recapController == null)
                {
                    var songBasicDetailsRepository = RepositoryContext.SongBasicDetailsRepositoryInstance;
                    var userPlaybackBehaviourRepository = RepositoryContext.UserPlaybackBehaviourRepositoryInstance;
                    recapController = new RecapController(songBasicDetailsRepository, userPlaybackBehaviourRepository);
                }
                return recapController;
            }
        }

        public static TopGenresController TopGenresControllerInstance
        {
            get
            {
                if (topGenresController == null)
                {
                    var songDabaseModelRepository = RepositoryContext.SongDataBaseModelRepositoryInstance;
                    var songRecommendationRepository = RepositoryContext.SongRecommendationDetailsRepositoryInstance;
                    topGenresController = new TopGenresController(songDabaseModelRepository, songRecommendationRepository);
                }
                return topGenresController;
            }
        }
        public static UserController UserControllerInstance
        {
            get
            {
                if (userController == null)
                {
                    var userSongRepository = RepositoryContext.UserSongRepositoryInstance;
                    userController = new UserController(userSongRepository);
                }
                return userController;
            }
        }
    }
}
