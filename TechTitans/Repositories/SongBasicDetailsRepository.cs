using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTitans.Models;

namespace TechTitans.Repositories
{
    /// <summary>
    /// Represents a repository for managing song basic details,
    /// including operations for retrieving song information, top listened songs, 
    /// and user playback behavior analysis.
    /// </summary>
    internal class SongBasicDetailsRepository : Repository<SongDataBaseModel>
    {

        public SongBasicDetailsRepository(IConfiguration configuration) : base(configuration)
        {
        }
        /// <summary>
        /// Converts song basic details to a simplified song information model.
        /// </summary>
        /// <param name="songBasicDetails">The song basic details to convert.</param>
        /// <returns>A simplified song information model.</returns>
        public SongBasicInformation SongBasicDetailsToSongBasicInfo(SongDataBaseModel songBasicDetails)
        {
            var artistId = songBasicDetails.Artist_Id;
            using (var connection = _databaseHelper.GetConnection())
            {
                connection.Open();
                var artistName = connection.QueryFirstOrDefault<string>("SELECT name FROM AuthorDetails WHERE artist_id = @artistId", new { artistId });
                return new SongBasicInformation
                {
                    SongId = songBasicDetails.Song_Id,
                    Name = songBasicDetails.Name,
                    Genre = songBasicDetails.Genre,
                    Subgenre = songBasicDetails.Subgenre,
                    Artist = artistName,
                    Language = songBasicDetails.Language,
                    Country = songBasicDetails.Country,
                    Album = songBasicDetails.Album,
                    Image = songBasicDetails.Image
                };
            }
        }

        /// <summary>
        /// Retrieves the basic details of a song by its ID.
        /// </summary>
        /// <param name="songId">The ID of the song.</param>
        /// <returns>The song basic details.</returns>
        public SongDataBaseModel GetSongBasicDetails(int songId)
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                connection.Open();
                return connection.QueryFirstOrDefault<SongDataBaseModel>("SELECT * FROM SongBasicDetails WHERE song_id = @songId", new { songId });
            }
        }

        /// <summary>
        /// Retrieves the top 5 most listened songs for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of the top 5 most listened songs.</returns>
         public List<SongDataBaseModel> GetTop5MostListenedSongs(int userId)
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                connection.Open();
                return connection.Query<SongDataBaseModel>(@"SELECT * FROM SongBasicDetails WHERE song_id IN (
                    SELECT TOP 5 song_id FROM UserPlaybackBehaviour WHERE user_id = @userId AND event_type = 2 
                    GROUP BY song_id ORDER BY COUNT(song_id) DESC)", new { userId }).AsList();
            }
        }

        /// <summary>
        /// Retrieves the percentile of the most played song for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A tuple containing the most played song and its percentile.</returns>
       public Tuple<SongDataBaseModel, decimal> GetMostPlayedSongPercentile(int userId)
        {
            SongDataBaseModel mostPlayedSong;
            decimal percentile;
            using (var connection = _databaseHelper.GetConnection())
            {
                connection.Open();
                mostPlayedSong = GetMostPlayedSong(userId);
                int totalSongs = GetTotalSongsPlayedByUser(userId);
                int mostListenedSongCount = GetMostListenedSongCount(userId);
                percentile = (decimal)mostListenedSongCount / totalSongs;
            }
            return Tuple.Create(mostPlayedSong, percentile);
        }

        /// <summary>
        /// Retrieves the most played song for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The most played song.</returns>
        private SongDataBaseModel GetMostPlayedSong(int userId)
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                connection.Open();
                return connection.QueryFirstOrDefault<SongDataBaseModel>(@"SELECT * FROM SongBasicDetails WHERE song_id IN (
                    SELECT TOP 1 song_id FROM UserPlaybackBehaviour WHERE user_id = @userId AND event_type = 2 AND YEAR(timestamp) = YEAR(GETDATE()) 
                    GROUP BY song_id ORDER BY COUNT(song_id) DESC)", new { userId });
            }
        }

        /// <summary>
        /// Retrieves the total number of songs played by a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The total number of songs played.</returns>
        private int GetTotalSongsPlayedByUser(int userId)
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                connection.Open();
                return connection.QueryFirstOrDefault<int>(@"SELECT COUNT(*) FROM UserPlaybackBehaviour WHERE user_id = @userId 
                AND event_type = 2 AND YEAR(timestamp) = YEAR(GETDATE())", new { userId });
            }
        }

        /// <summary>
        /// Retrieves the count of the most listened song for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The count of the most listened song.</returns>
        private int GetMostListenedSongCount(int userId)
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                connection.Open();
                return connection.QueryFirstOrDefault<int>(@"SELECT COUNT(*) FROM UserPlaybackBehaviour WHERE user_id = @userId 
                AND event_type = 2 AND YEAR(timestamp) = YEAR(GETDATE()) AND song_id IN (
                SELECT TOP 1 song_id FROM UserPlaybackBehaviour WHERE user_id = @userId AND event_type = 2 
                AND YEAR(timestamp) = YEAR(GETDATE()) GROUP BY song_id ORDER BY COUNT(song_id) DESC)", new { userId });
            }
        }

        /// <summary>
        /// Retrieves the percentile of the most played artist for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>Percentile of the most played artist.</returns>
        public Tuple<string, decimal> GetMostPlayedArtistPercentile(int userId)
        {
            var mostPlayedArtistInfo = GetMostPlayedArtistInfo(userId);
            
            var mostPlayedArtist = GetMostPlayedArtist(userId, mostPlayedArtistInfo);
            
            var totalSongs = GetTotalNumberOfSongs(userId);
            
            return new Tuple<string, decimal>(mostPlayedArtist, (decimal)mostPlayedArtistInfo.Start_Listen_Events / totalSongs);
        }

        /// <summary>
        /// Retrieves information about the most played artist for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>Information about the most played artist.</returns>
        private MostPlayedArtistInformation GetMostPlayedArtistInfo(int userId)
        {
            MostPlayedArtistInformation mostPlayedArtistInfo;
            using (var connection = _databaseHelper.GetConnection())
            {
                connection.Open();
                var queryBuilder = new StringBuilder();
                queryBuilder.Append(@"SELECT TOP 1 sd.artist_id as Artist_Id, COUNT(*) AS Start_Listen_Events 
                                      FROM UserPlaybackBehaviour ub 
                                      JOIN SongBasicDetails sd ON ub.song_id = sd.song_id 
                                      WHERE ub.user_id = @userId AND ub.event_type = 2 AND YEAR(timestamp) = YEAR(GETDATE()) 
                                      GROUP BY sd.artist_id 
                                      ORDER BY COUNT(*) DESC");
                mostPlayedArtistInfo = connection.QueryFirstOrDefault<MostPlayedArtistInformation>(queryBuilder.ToString(), new { userId });
            }
            return mostPlayedArtistInfo;
        }

        /// <summary>
        /// Retrieves the name of the most played artist for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="mostPlayedArtistInfo">Information about the most played artist.</param>
        /// <returns>The name of the most played artist.</returns>
        private string GetMostPlayedArtist(int userId, MostPlayedArtistInformation mostPlayedArtistInfo)
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                connection.Open();
                return connection.QueryFirstOrDefault<string>("SELECT name FROM AuthorDetails WHERE artist_id = @artist_Id", new { mostPlayedArtistInfo.Artist_Id });
            }
        }

        /// <summary>
        /// Retrieves the total number of songs for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The total number of songs.</returns>
        private int GetTotalNumberOfSongs(int userId)
        {
            int totalSongs;
            using (var connection = _databaseHelper.GetConnection())
            {
                connection.Open();
                var queryBuilder = new StringBuilder();
                queryBuilder.Append(@"SELECT COUNT(*) 
                                      FROM UserPlaybackBehaviour 
                                      WHERE user_id = @userId AND event_type = 2 AND YEAR(timestamp) = YEAR(GETDATE())");
                totalSongs = connection.QueryFirstOrDefault<int>(queryBuilder.ToString(), new { userId });
            }
            return totalSongs;
        }

        /// <summary>
        /// Retrieves the top 5 genres for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of the top 5 genres.</returns>
        public List<string> GetTop5Genres(int userId)
        {
            List<string> topGenres;
            using (var connection = _databaseHelper.GetConnection())
            {
                connection.Open();
                var queryBuilder = new StringBuilder();
                queryBuilder.Append(@"SELECT TOP 5 sb.genre 
                                      FROM UserPlaybackBehaviour ub 
                                      JOIN SongBasicDetails sb ON ub.song_id = sb.song_id 
                                      WHERE ub.user_id = @userId AND ub.event_type = 2 AND YEAR(ub.timestamp) = YEAR(GETDATE()) 
                                      GROUP BY sb.genre 
                                      ORDER BY COUNT(*) DESC");
                topGenres = connection.Query<string>(queryBuilder.ToString(), new { userId }).ToList();
            }
            return topGenres;
        }

        /// <summary>
        /// Retrieves a list of distinct genres that have been played in the current year but not in the previous year for a specified user.
        /// </summary>
        /// <param name="userId">The ID of the user for whom to retrieve the new genres.</param>
        /// <returns>A list of strings representing the new genres discovered by the user in the current year.</returns>
        public List<string> GetAllNewGenresDiscovered(int userId)
        {
            List<string> newGenres;
            using (var connection = _databaseHelper.GetConnection())
            {
                connection.Open();
                var queryBuilder = new StringBuilder();
                queryBuilder.Append(@"SELECT DISTINCT genre 
                                      FROM SongBasicDetails 
                                      WHERE song_id IN (SELECT song_id FROM UserPlaybackBehaviour WHERE user_id = @userId AND event_type = 2 AND YEAR(timestamp) = YEAR(GETDATE()) GROUP BY song_id) 
                                      AND genre NOT IN (SELECT genre FROM SongBasicDetails WHERE song_id IN (SELECT song_id FROM UserPlaybackBehaviour WHERE user_id = @userId AND event_type = 2 AND YEAR(timestamp) = YEAR(GETDATE()) - 1 GROUP BY song_id))");
                newGenres = connection.Query<string>(queryBuilder.ToString(), new { userId }).ToList();
            }
            return newGenres;
        }
    }
}
