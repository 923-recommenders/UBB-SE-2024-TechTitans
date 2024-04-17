using Dapper;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTitans.Models;

namespace TechTitans.Repositories
{
    internal class SongBasicDetailsRepository : Repository<SongDataBaseModel>
    {
        public SongBasicInformation SongBasicDetailsToSongBasicInfo(SongDataBaseModel songBasicDetails)
        {   
            var artistId = songBasicDetails.Artist_Id;
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT name FROM AuthorDetails WHERE artist_id = @artistId");
            var artistName = _connection.Query<string>(queryBuilder.ToString(), new { artistId }).FirstOrDefault();
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
        
        public SongDataBaseModel GetSongBasicDetails(int songId)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM SongBasicDetails WHERE song_id = @songId");
            return _connection.Query<SongDataBaseModel>(queryBuilder.ToString(), new { songId }).FirstOrDefault();
        }

        public List<SongDataBaseModel> GetTop5MostListenedSongs(int userId)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM SongBasicDetails WHERE song_id IN (SELECT TOP 5 song_id FROM UserPlaybackBehaviour WHERE user_id = @userId AND event_type = 2 GROUP BY song_id ORDER BY COUNT(song_id) DESC);");
            return _connection.Query<SongDataBaseModel>(queryBuilder.ToString(), new { userId }).ToList();
        }

        public Tuple<SongDataBaseModel, decimal> GetMostPlayedSongPercentile(int userId)
        {
            var mostPlayedSong = GetMostPlayedSong(userId);
            var totalSongs = GetTotalSongsPlayedByUser(userId);
            var mostListenedSongCount = GetMostListenedSongCount(userId);
            return new Tuple<SongDataBaseModel, decimal>(mostPlayedSong, (decimal)mostListenedSongCount / totalSongs);
        }

        private SongDataBaseModel GetMostPlayedSong(int userId)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM SongBasicDetails WHERE song_id IN (SELECT TOP 1 song_id FROM UserPlaybackBehaviour WHERE user_id = @userId AND event_type = 2 AND YEAR(timestamp) = YEAR(GETDATE()) GROUP BY song_id ORDER BY COUNT(song_id) DESC);");
            return _connection.Query<SongDataBaseModel>(queryBuilder.ToString(), new { userId }).FirstOrDefault();
        }

        private int GetTotalSongsPlayedByUser(int userId)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT COUNT(*) FROM UserPlaybackBehaviour WHERE user_id = @userId AND event_type = 2 AND YEAR(timestamp) = YEAR(GETDATE());");
            return _connection.Query<int>(queryBuilder.ToString(), new { userId }).FirstOrDefault();
        }

        private int GetMostListenedSongCount(int userId)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT COUNT(*) FROM UserPlaybackBehaviour WHERE user_id = @userId AND event_type = 2 AND YEAR(timestamp) = YEAR(GETDATE()) AND song_id IN (SELECT TOP 1 song_id FROM UserPlaybackBehaviour WHERE user_id = @userId AND event_type = 2 AND YEAR(timestamp) = YEAR(GETDATE()) GROUP BY song_id ORDER BY COUNT(song_id) DESC);");
            return _connection.Query<int>(queryBuilder.ToString(), new { userId }).FirstOrDefault();
        }

        public Tuple<string, decimal> GetMostPlayedArtistPercentile(int userId)
        {
            var mostPlayedArtistInfo = GetMostPlayedArtistInfo(userId);
            
            var mostPlayedArtist = GetMostPlayedArtist(userId, mostPlayedArtistInfo);
            
            var totalSongs = GetTotalNumberOfSongs(userId);
            
            return new Tuple<string, decimal>(mostPlayedArtist, (decimal)mostPlayedArtistInfo.Start_Listen_Events / totalSongs);
        }

        private MostPlayedArtistInformation GetMostPlayedArtistInfo(int userId)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT TOP 1 sd.artist_id as Artist_Id, COUNT(*) AS Start_Listen_Events FROM UserPlaybackBehaviour ub JOIN SongBasicDetails sd ON ub.song_id = sd.song_id WHERE ub.user_id = @userId AND ub.event_type = 2 AND YEAR(timestamp) = YEAR(GETDATE()) GROUP BY sd.artist_id ORDER BY COUNT(*) DESC;");
            return _connection.Query<MostPlayedArtistInformation>(queryBuilder.ToString(), new { userId }).FirstOrDefault();
        }
        private string GetMostPlayedArtist(int userId, MostPlayedArtistInformation mostPlayedArtistInfo)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT name FROM AuthorDetails WHERE artist_id = @artist_Id");
            return _connection.Query<string>(queryBuilder.ToString(), new { mostPlayedArtistInfo.Artist_Id }).FirstOrDefault();
        }
        private int GetTotalNumberOfSongs(int userId)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT COUNT(*) FROM UserPlaybackBehaviour WHERE user_id = @userId AND event_type = 2 AND YEAR(timestamp) = YEAR(GETDATE());");
            return _connection.Query<int>(queryBuilder.ToString(), new { userId }).FirstOrDefault();
        }
        public List<string> GetTop5Genres(int userId)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT TOP 5 sb.genre FROM UserPlaybackBehaviour ub JOIN SongBasicDetails sb ON ub.song_id = sb.song_id WHERE ub.user_id = @userId AND ub.event_type = 2 AND YEAR(ub.timestamp) = YEAR(GETDATE()) GROUP BY sb.genre ORDER BY COUNT(*) DESC;");
            return _connection.Query<string>(queryBuilder.ToString(), new { userId }).ToList();
        }

        /// <summary>
        /// Retrieves a list of distinct genres that have been played in the current year but not in the previous year for a specified user.
        /// </summary>
        /// <param name="userId">The ID of the user for whom to retrieve the new genres.</param>
        /// <returns>A list of strings representing the new genres discovered by the user in the current year.</returns>
        public List<string> GetAllNewGenresDiscovered(int userId)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT DISTINCT genre FROM SongBasicDetails WHERE song_id IN (SELECT song_id FROM UserPlaybackBehaviour WHERE user_id = @userId AND event_type = 2 AND YEAR(timestamp) = YEAR(GETDATE()) GROUP BY song_id) AND genre NOT IN (SELECT genre FROM SongBasicDetails WHERE song_id IN (SELECT song_id FROM UserPlaybackBehaviour WHERE user_id = @userId AND event_type = 2 AND YEAR(timestamp) = YEAR(GETDATE()) - 1 GROUP BY song_id));");
            return _connection.Query<string>(queryBuilder.ToString(), new { userId }).ToList();
        }
    }
}
