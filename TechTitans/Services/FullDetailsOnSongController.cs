using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTitans.Models;
using TechTitans.Repositories;
using TechTitans.Enums;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace TechTitans.Services
{
    /// <summary>
    /// Provides functionality for retrieving detailed information about songs,
    /// including playback behavior and ad distribution data.
    /// </summary>
    internal class FullDetailsOnSongController
    {
        private static readonly IConfiguration _configuration = MauiProgram.Configuration;
        private static IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(_configuration.GetConnectionString("TechTitansDev"));
        private static IDatabaseOperations databaseOperations = new DatabaseOperations(connection);
        private readonly Repository<UserPlaybackBehaviour> UserPlaybackBehaviourRepo = new Repository<UserPlaybackBehaviour>(databaseOperations);
        private readonly Repository<AdDistributionData> AdDistributionDataRepo = new Repository<AdDistributionData>(databaseOperations);

        /// <summary>
        /// Retrieves full details on a song, including total minutes listened,
        /// total plays, likes, dislikes, and skips.
        /// </summary>
        /// <param name="songId">The ID of the song.</param>
        /// <returns>A <see cref="FullDetailsOnSong"/> object containing the detailed 
        /// information about the song, or null if the song is not found.</returns>
        public FullDetailsOnSong GetFullDetailsOnSong(int songId) {
            FullDetailsOnSong currentSongDetails = new();
            DateTime start = new(); 
            bool foundSongCheck = false;
            foreach (UserPlaybackBehaviour action in UserPlaybackBehaviourRepo.GetAll())
            {

                if (action.Song_Id == songId)
                {
                    foundSongCheck = true;

                    switch (action.Event_Type)
                    {
                        case PlaybackEventType.startSongPlayback:
                            start = action.Timestamp;
                            break;
                        case PlaybackEventType.endSongPlayback:
                            int minutes = (action.Timestamp - start).Minutes;
                            currentSongDetails.TotalMinutesListened += minutes;
                            currentSongDetails.TotalPlays++;
                            break;
                        case PlaybackEventType.like:
                            currentSongDetails.TotalLikes++;
                            break;
                        case PlaybackEventType.dislike:
                            currentSongDetails.TotalDislikes++;
                            break;
                        case PlaybackEventType.skip:
                            currentSongDetails.TotalSkips++;
                            break;

                    }
                }
            }
            if (!foundSongCheck) {
                
                return null;
            }
            return currentSongDetails;
        }

        /// <summary>
        /// Retrieves the details of a song for the current month,
        /// including total minutes listened, total plays, likes, dislikes,
        /// and skips.
        /// </summary>
        /// <param name="songId">The ID of the song.</param>
        /// <returns>A <see cref="FullDetailsOnSong"/> object containing the 
        /// detailed information about the song for the current month.</returns>
        public FullDetailsOnSong GetCurrentMonthDetails(int songId) {
            FullDetailsOnSong currentSongDetails = new();
            foreach (UserPlaybackBehaviour action in UserPlaybackBehaviourRepo.GetAll()) {
                if (action.Song_Id == songId && action.Timestamp.Month == DateTime.Now.Month && action.Timestamp.Year == DateTime.Now.Year) {
                    switch (action.Event_Type) {
                        case PlaybackEventType.startSongPlayback:
                            break;
                        case PlaybackEventType.endSongPlayback:
                            int minutes = (action.Timestamp - DateTime.Now).Minutes;
                            currentSongDetails.TotalMinutesListened += minutes;
                            currentSongDetails.TotalPlays++;
                            break;
                        case PlaybackEventType.like:
                            currentSongDetails.TotalLikes++;
                            break;
                        case PlaybackEventType.dislike:
                            currentSongDetails.TotalDislikes++;
                            break;
                        case PlaybackEventType.skip:
                            currentSongDetails.TotalSkips++;
                            break;
                    }
                }
            }
            return currentSongDetails;
        }

        /// <summary>
        /// Retrieves the active ad distribution data for a specific song.
        /// </summary>
        /// <param name="songId">The ID of the song.</param>
        /// <returns>An <see cref="AdDistributionData"/> object containing
        /// the active ad distribution data for the song, or null
        /// if no active ad is found.</returns>
        public AdDistributionData GetActiveAd(int songId) {
            foreach (AdDistributionData ad in AdDistributionDataRepo.GetAll()) {
                if (ad.Song_Id == songId && ad.Month == DateTime.Now.Month && ad.Year == DateTime.Now.Year) {
                    return ad;
                }
            }
            return null;
        }
    }
}
