using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTitans.Repositories;
using TechTitans.Models;
using Dapper;
using TechTitans.ViewModels;
using TechTitans.Enums;

namespace TechTitans.Services
{   
    internal class RecapController
    {
        SongBasicDetailsRepository songBasicDetailsRepository = new SongBasicDetailsRepository();
        UserPlaybackBehaviourRepository userPlaybackBehaviourRepository = new UserPlaybackBehaviourRepository();

        public List<SongBasicInformation> Top5MostListenedSongs(int userId)
        {
            var top5Songs = songBasicDetailsRepository.GetTop5MostListenedSongs(userId);
            List<SongBasicInformation> top5SongsInfo = new List<SongBasicInformation>();
            foreach (var song in top5Songs)
            {
                top5SongsInfo.Add(songBasicDetailsRepository.SongBasicDetailsToSongBasicInfo(song));
            }
            return top5SongsInfo;
        }

        public Tuple<SongBasicInformation, decimal> MostPlayedSongPercentile(int userId)
        {
            var mostPlayedSong = songBasicDetailsRepository.GetMostPlayedSongPercentile(userId);
            return new Tuple<SongBasicInformation, decimal>(songBasicDetailsRepository.SongBasicDetailsToSongBasicInfo(mostPlayedSong.Item1), mostPlayedSong.Item2);
        }

        public Tuple<string, decimal> MostPlayedArtistPercentile(int userId)
        {
            return songBasicDetailsRepository.GetMostPlayedArtistPercentile(userId);
        }

        /// <summary>
        /// Calculates the total minutes listened by a user based on their playback behavior.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The total minutes listened by the user.</returns>
        public int MinutesListened(int userId)
        {
            var userEvents = userPlaybackBehaviourRepository.GetUserPlaybackBehaviour(userId);
            int minutesListened = 0;
            for (int firstCounter = 0; firstCounter < userEvents.Count; firstCounter++)
            {
                if (userEvents[firstCounter].Event_Type == PlaybackEventType.start_play)
                {
                    for (int secondCounter = firstCounter + 1; secondCounter < userEvents.Count; secondCounter++)
                    {
                        if (userEvents[secondCounter].Event_Type == PlaybackEventType.end_play)
                        {
                            minutesListened += (int)(userEvents[secondCounter].Timestamp - userEvents[firstCounter].Timestamp).TotalMinutes;
                            firstCounter = secondCounter;
                            break;
                        }
                    }
                }
            }
            return minutesListened;
        }

        public List<string> Top5Genres(int userId)
        {
            return this.songBasicDetailsRepository.GetTop5Genres(userId);
        }

        public List<string> NewGenresDiscovered(int userId)
        {
            return this.songBasicDetailsRepository.GetAllNewGenresDiscovered(userId);
        }

        public ListenerPersonality ListenerPersonality(int userId)
        {
            var userEvents = userPlaybackBehaviourRepository.GetUserPlaybackBehaviour(userId);
            int playCount = 0;
            for(int counter = 0; counter < userEvents.Count; counter++)
            {
                if ((userEvents[counter].Event_Type == PlaybackEventType.start_play) && (userEvents[counter].Timestamp.Year == DateTime.Now.Year))
                {
                    playCount++;
                }
            }
            if(playCount > 100)
            {
                return Enums.ListenerPersonality.Melophile;
            }
            var newGenres = NewGenresDiscovered(userId);
            if(newGenres.Count > 3)
            {
                return Enums.ListenerPersonality.Explorer;
            }
            if(playCount < 10)
            {
                return Enums.ListenerPersonality.Casual;
            }
            return Enums.ListenerPersonality.Vanilla;
        }

        public EndOfYearRecapViewModel GenerateEndOfYearRecap(int userId)
        {
            var endOfYearRecap = new EndOfYearRecapViewModel();
            endOfYearRecap.Top5MostListenedSongs = Top5MostListenedSongs(userId);
            endOfYearRecap.MostPlayedSongPercentile = MostPlayedSongPercentile(userId);
            endOfYearRecap.MostPlayedArtistPercentile = MostPlayedArtistPercentile(userId);
            endOfYearRecap.MinutesListened = MinutesListened(userId);
            endOfYearRecap.Top5Genres = Top5Genres(userId);
            endOfYearRecap.NewGenresDiscovered = NewGenresDiscovered(userId);
            endOfYearRecap.ListenerPersonality = ListenerPersonality(userId);
            return endOfYearRecap;
        }
    }
}
