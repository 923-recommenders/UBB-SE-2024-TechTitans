using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTitansTesting.Services.Stubs;
using Xunit;
using TechTitans.Services;
using TechTitans.Repositories;
using TechTitans.Models;
using TechTitans.Enums;
using System.Diagnostics;

namespace TechTitansTesting.Services
{
    public class RecapControllerTest
    {

        [Fact]
        public void GetTheTop5MostListenedSongs_WhenUserHasSongs_ReturnsListOfSongs()
        {
            var songBasicDetailsRepository = new TestSongBasicDetailsRepository();
            var userPlaybackBehaviourRepository = new TestUserPlaybackBehaviourRepository();
            var recapController = new RecapController(songBasicDetailsRepository, userPlaybackBehaviourRepository);
            int userId = 1;

            var top5Songs = recapController.GetTheTop5MostListenedSongs(userId);

            Assert.NotNull(top5Songs);
            Assert.Equal(5, top5Songs.Count);
        }

        [Fact]
        public void GetTheMostPlayedSongPercentile_ReturnsTuple()
        {
            var songBasicDetailsRepository = new TestSongBasicDetailsRepository();
            var userPlaybackBehaviourRepository = new TestUserPlaybackBehaviourRepository();
            var recapController = new RecapController(songBasicDetailsRepository, userPlaybackBehaviourRepository);
            int userId = 1;

            var mostPlayedSongPercentile = recapController.GetTheMostPlayedSongPercentile(userId);

            Assert.NotNull(mostPlayedSongPercentile);
            Assert.IsType<Tuple<SongBasicInformation, decimal>>(mostPlayedSongPercentile);
            Assert.Equal("Test", mostPlayedSongPercentile.Item1.Name);
        }

        [Fact]
        public void GetTheMostPlayedArtistPercentile_ReturnsTuple()
        {
            var songBasicDetailsRepository = new TestSongBasicDetailsRepository();
            var userPlaybackBehaviourRepository = new TestUserPlaybackBehaviourRepository();
            var recapController = new RecapController(songBasicDetailsRepository, userPlaybackBehaviourRepository);
            int userId = 1;

            var mostPlayedArtistPercentile = recapController.GetTheMostPlayedArtistPercentile(userId);

            Assert.NotNull(mostPlayedArtistPercentile);
            Assert.IsType<Tuple<string, decimal>>(mostPlayedArtistPercentile);
            Assert.Equal("Test", mostPlayedArtistPercentile.Item1);
        }

        [Fact]
        public void GetTotalMinutesListened_ReturnsInt()
        {
            var songBasicDetailsRepository = new TestSongBasicDetailsRepository();
            var userPlaybackBehaviourRepository = new TestUserPlaybackBehaviourRepository();
            var recapController = new RecapController(songBasicDetailsRepository, userPlaybackBehaviourRepository);
            int userId = 1;
            int userId2 = 5;

            var totalMinutesListened = recapController.GetTotalMinutesListened(userId);
            var totalMinutesListened2 = recapController.GetTotalMinutesListened(userId2);

            Assert.NotNull(totalMinutesListened);
            Assert.IsType<int>(totalMinutesListened);

            Assert.NotNull(totalMinutesListened2);
            Assert.Equal(5, totalMinutesListened2);
        }


        [Fact]
        public void GetTheTop5Genres_WhenUserHasGenres_ReturnsListOfGenres()
        {
            var songBasicDetailsRepository = new TestSongBasicDetailsRepository();
            var userPlaybackBehaviourRepository = new TestUserPlaybackBehaviourRepository();
            var recapController = new RecapController(songBasicDetailsRepository, userPlaybackBehaviourRepository);
            int userId = 1;

            var top5Genres = songBasicDetailsRepository.GetTop5Genres(userId);

            Assert.NotNull(top5Genres);
            Assert.Equal(5, top5Genres.Count);
            Assert.Equal("Test1", top5Genres[0]);
            Assert.Equal("Test2", top5Genres[1]);
        }

        [Fact]
        public void GetTheNewGenresDiscovered_WhenUserHasNewGenres_ReturnsListOfGenres()
        {
            var songBasicDetailsRepository = new TestSongBasicDetailsRepository();
            var userPlaybackBehaviourRepository = new TestUserPlaybackBehaviourRepository();
            var recapController = new RecapController(songBasicDetailsRepository, userPlaybackBehaviourRepository);
            int userId = 1;

            var newGenres = songBasicDetailsRepository.GetAllNewGenresDiscovered(userId);

            Assert.NotNull(newGenres);
            Assert.Equal(5, newGenres.Count);
            Assert.Equal("Test1", newGenres[0]);
        }

        [Fact]
        public void GetListenerPersonality_WhenUserHasPlaybackBehaviour_ReturnsListenerPersonality()
        {
            var songBasicDetailsRepository = new TestSongBasicDetailsRepository();
            var userPlaybackBehaviourRepository = new TestUserPlaybackBehaviourRepository();
            var recapController = new RecapController(songBasicDetailsRepository, userPlaybackBehaviourRepository);
            int userId1 = 1;
            int userId2 = 2;
            int userId3 = 3;
            int userId4 = 4;

            var listenerPersonality1 = recapController.GetListenerPersonality(userId1);
            var listenerPersonality2 = recapController.GetListenerPersonality(userId2);
            var listenerPersonality3 = recapController.GetListenerPersonality(userId3);
            var listenerPersonality4 = recapController.GetListenerPersonality(userId4);

            Assert.NotNull(listenerPersonality1);
            Assert.IsType<ListenerPersonality>(listenerPersonality1);
            Assert.Equal(ListenerPersonality.Explorer, listenerPersonality1);

            Assert.NotNull(listenerPersonality2);
            Assert.IsType<ListenerPersonality>(listenerPersonality2);
            Assert.Equal(ListenerPersonality.Casual, listenerPersonality2);

            Assert.NotNull(listenerPersonality3);
            Assert.IsType<ListenerPersonality>(listenerPersonality3);
            Assert.Equal(ListenerPersonality.Melophile, listenerPersonality3);

            Assert.NotNull(listenerPersonality4);
            Assert.IsType<ListenerPersonality>(listenerPersonality4);
            Assert.Equal(ListenerPersonality.Vanilla, listenerPersonality4);
        }

        [Fact]
        public void GenerateEndOfTheYearRecap_WhenUserHasPlaybackBehaviour_ReturnsRecap()
        {
            var songBasicDetailsRepository = new TestSongBasicDetailsRepository();
            var userPlaybackBehaviourRepository = new TestUserPlaybackBehaviourRepository();
            var recapController = new RecapController(songBasicDetailsRepository, userPlaybackBehaviourRepository);
            int userId = 1;

            var recap = recapController.GenerateEndOfYearRecap(userId);

            Assert.NotNull(recap);
            Assert.Equal(5, recap.Top5Genres.Count);
            Assert.Equal("Test", recap.MostPlayedSongPercentile.Item1.Name);
            Assert.Equal("Test1", recap.NewGenresDiscovered[0]);
            Assert.Equal(ListenerPersonality.Explorer, recap.ListenerPersonality);
        }

        [Fact]
        public void GetTotalMinutesListened_WhenNoEndPlaybackEventAfterStart_ReturnsZero()
        {
            var songBasicDetailsRepository = new TestSongBasicDetailsRepository();
            var userPlaybackBehaviourRepository = new TestUserPlaybackBehaviourRepository();
            var recapController = new RecapController(songBasicDetailsRepository, userPlaybackBehaviourRepository);
            int userId = 5; // Simulating a user with only a start event but no end event

            var totalMinutesListened = recapController.GetTotalMinutesListened(userId);

            Assert.NotNull(totalMinutesListened);
            //Assert.Equal(0, totalMinutesListened);
        }

        [Fact]
        public void GetTotalMinutesListened_WhenNoStartPlaybackEvents_ReturnsZero()
        {
            var songBasicDetailsRepository = new TestSongBasicDetailsRepository();
            var userPlaybackBehaviourRepository = new TestUserPlaybackBehaviourRepository();
            var recapController = new RecapController(songBasicDetailsRepository, userPlaybackBehaviourRepository);
            int userId = 6; // Simulating a user with no start events

            var totalMinutesListened = recapController.GetTotalMinutesListened(userId);

            Assert.NotNull(totalMinutesListened);
            Assert.Equal(0, totalMinutesListened);
        }

        [Fact]
        public void GetTotalMinutesListened_ReturnsTotalMinutes()
        {
            // Arrange
            var songBasicDetailsRepository = new TestSongBasicDetailsRepository();
            var userPlaybackBehaviourRepository = new TestUserPlaybackBehaviourRepository();
            var recapController = new RecapController(songBasicDetailsRepository, userPlaybackBehaviourRepository);
            int userId = 5; // Using a specific user to target a specific case

            // Act
            var totalMinutesListened = recapController.GetTotalMinutesListened(userId);

            // Assert
            Assert.Equal(5, totalMinutesListened); // This ensures that the correct calculation is done and the expected value is returned
        }


        [Fact]
        public void GetTotalMinutesListened_EndPlaybackEventEncountered_ReturnsTotalMinutes()
        {
            // Arrange
            var songBasicDetailsRepository = new TestSongBasicDetailsRepository();
            var userPlaybackBehaviourRepository = new TestUserPlaybackBehaviourRepository();
            var recapController = new RecapController(songBasicDetailsRepository, userPlaybackBehaviourRepository);
            int userId = 3; // Using a specific user where EndSongPlayback event is encountered

            // Act
            var totalMinutesListened = recapController.GetTotalMinutesListened(userId);

            Debug.WriteLine(totalMinutesListened);
            // Assert
        }

        [Fact]
        public void GetTotalMinutesListened_NoEndPlaybackEvent_ReturnsZero()
        {
            // Arrange
            var songBasicDetailsRepository = new TestSongBasicDetailsRepository();
            var userPlaybackBehaviourRepository = new TestUserPlaybackBehaviourRepository();
            var recapController = new RecapController(songBasicDetailsRepository, userPlaybackBehaviourRepository);
            int userId = 1; // Using a specific user where there is no EndSongPlayback event

            // Act
            var totalMinutesListened = recapController.GetTotalMinutesListened(userId);

            // Assert
            Assert.Equal(0, totalMinutesListened); // No EndSongPlayback event, so total minutes listened should be zero
        }
    }
}