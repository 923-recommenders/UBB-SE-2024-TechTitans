﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTitans.Repositories;
using TechTitans.Models;
using Dapper;
using TechTitans.ViewModels;
using TechTitans.Enums;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace TechTitans.Services
{
    /// <summary>
    /// Provides functionality for generating recaps based on 
    /// user playback behavior and song details.
    /// </summary>
    internal class RecapController
    {
        private static readonly IConfiguration _configuration = MauiProgram.Configuration;
        private static IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(_configuration.GetConnectionString("TechTitansDev"));
        private static IDatabaseOperations databaseOperations = new DatabaseOperations(connection);
        SongBasicDetailsRepository songBasicDetailsRepository = new SongBasicDetailsRepository(databaseOperations);
        UserPlaybackBehaviourRepository userPlaybackBehaviourRepository = new UserPlaybackBehaviourRepository(databaseOperations);

        /// <summary>
        /// Retrieves the top 5 most listened songs for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of the top 5 most listened songs.</returns>
        public List<SongBasicInformation> GetTheTop5MostListenedSongs(int userId)
        {
            var top5Songs = songBasicDetailsRepository.GetTop5MostListenedSongs(userId);
            List<SongBasicInformation> top5SongsInformation = new List<SongBasicInformation>();
            foreach (var song in top5Songs)
            {
                top5SongsInformation.Add(songBasicDetailsRepository.TransformSongBasicDetailsToSongBasicInfo(song));
            }
            return top5SongsInformation;
        }

        /// <summary>
        /// Retrieves the percentile of the most played song for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A tuple containing the most played song
        /// and its percentile.</returns>
        public Tuple<SongBasicInformation, decimal> GetTheMostPlayedSongPercentile(int userId)
        {
            var mostPlayedSong = songBasicDetailsRepository.GetMostPlayedSongPercentile(userId);
            return new Tuple<SongBasicInformation, decimal>(songBasicDetailsRepository.TransformSongBasicDetailsToSongBasicInfo(mostPlayedSong.Item1), mostPlayedSong.Item2);
        }

        /// <summary>
        /// Retrieves the percentile of the most played artist for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A tuple containing the most played artist and its percentile.</returns>
        public Tuple<string, decimal> GetTheMostPlayedArtistPercentile(int userId)
        {
            return songBasicDetailsRepository.GetMostPlayedArtistPercentile(userId);
        }

        /// <summary>
        /// Calculates the total minutes listened by a user based on their playback behavior.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The total minutes listened by the user.</returns>
        public int GetTotalMinutesListened(int userId)
        {
            var userEvents = userPlaybackBehaviourRepository.GetUserPlaybackBehaviour(userId);
            int totalMinutesListened = 0;
            for (int firstCounter = 0; firstCounter < userEvents.Count; firstCounter++)
            {
                if (userEvents[firstCounter].Event_Type == PlaybackEventType.startSongPlayback)
                {
                    for (int secondCounter = firstCounter + 1; secondCounter < userEvents.Count; secondCounter++)
                    {
                        if (userEvents[secondCounter].Event_Type == PlaybackEventType.endSongPlayback)
                        {
                            totalMinutesListened += (int)(userEvents[secondCounter].Timestamp - userEvents[firstCounter].Timestamp).TotalMinutes;
                            firstCounter = secondCounter;
                            break;
                        }
                    }
                }
            }
            return totalMinutesListened;
        }

        /// <summary>
        /// Retrieves the top 5 genres for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of the top 5 genres.</returns>
        public List<string> GetTheTop5Genres(int userId)
        {
            return this.songBasicDetailsRepository.GetTop5Genres(userId);
        }

        /// <summary>
        /// Retrieves new genres discovered by a user in the current year.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of new genres discovered by the user.</returns>
        public List<string> GetNewGenresDiscovered(int userId)
        {
            return this.songBasicDetailsRepository.GetAllNewGenresDiscovered(userId);
        }

        /// <summary>
        /// Determines the listener personality based on user
        /// playback behavior and genre discovery.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The listener personality of the user.</returns>
        public ListenerPersonality GetListenerPersonality(int userId)
        {
            var userEvents = userPlaybackBehaviourRepository.GetUserPlaybackBehaviour(userId);
            int playCount = 0;
            for(int counter = 0; counter < userEvents.Count; counter++)
            {
                if ((userEvents[counter].Event_Type == PlaybackEventType.startSongPlayback) && (userEvents[counter].Timestamp.Year == DateTime.Now.Year))
                {
                    playCount++;
                }
            }
            if(playCount > 100)
            {
                return Enums.ListenerPersonality.Melophile;
            }
            var newGenres = GetNewGenresDiscovered(userId);
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

        /// <summary>
        /// Generates an end-of-year recap for a user, including top songs, 
        /// artist percentiles, minutes listened, genres, and listener personality.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>An <see cref="EndOfYearRecapViewModel"/> containing 
        /// the end-of-year recap for the user.</returns>
        public EndOfYearRecapViewModel GenerateEndOfYearRecap(int userId)
        {
            var endOfYearRecap = new EndOfYearRecapViewModel();
            endOfYearRecap.Top5MostListenedSongs = GetTheTop5MostListenedSongs(userId);
            endOfYearRecap.MostPlayedSongPercentile = GetTheMostPlayedSongPercentile(userId);
            endOfYearRecap.MostPlayedArtistPercentile = GetTheMostPlayedArtistPercentile(userId);
            endOfYearRecap.MinutesListened = GetTotalMinutesListened(userId);
            endOfYearRecap.Top5Genres = GetTheTop5Genres(userId);
            endOfYearRecap.NewGenresDiscovered = GetNewGenresDiscovered(userId);
            endOfYearRecap.ListenerPersonality = GetListenerPersonality(userId);
            return endOfYearRecap;
        }
    }
}
