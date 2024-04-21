using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTitans.Models;

namespace TechTitans.Repositories
{
    /// <summary>
    /// Represents a repository for managing user playback behavior data, 
    /// including operations for retrieving playback behavior records.
    /// </summary>
    internal class UserPlaybackBehaviourRepository : Repository<UserPlaybackBehaviour>
    {
        public UserPlaybackBehaviourRepository(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Retrieves a specific user's playback behavior record 
        /// based on the provided criteria.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="songId">The optional ID of the song.</param>
        /// <param name="timestamp">The optional timestamp.</param>
        /// <returns>The user's playback behavior record matching
        /// the specified criteria, or null if no match is found.</returns>
        public UserPlaybackBehaviour GetUserPlaybackBehaviour(int userId, int? songId = null, DateTime? timestamp = null)
        {
            UserPlaybackBehaviour playbackBehaviour;
            using (var connection = _databaseHelper.GetConnection())
            {
                connection.Open();
                var queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT * FROM UserPlaybackBehaviour WHERE user_id = @userId");
                if (songId.HasValue)
                {
                    queryBuilder.Append(" AND song_id = @songId");
                }
                if (timestamp.HasValue)
                {
                    queryBuilder.Append(" AND timestamp = @timestamp");
                }
                playbackBehaviour = connection.QueryFirstOrDefault<UserPlaybackBehaviour>(queryBuilder.ToString(), new { userId, songId, timestamp });
            }
            return playbackBehaviour;
        }

        /// <summary>
        /// Retrieves all playback behavior records for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of playback behavior records for the specified user.</returns>
        public List<UserPlaybackBehaviour> GetUserPlaybackBehaviour(int userId)
        {
            List<UserPlaybackBehaviour> playbackBehaviourList;
            using (var connection = _databaseHelper.GetConnection())
            {
                connection.Open();
                var queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT user_id as User_Id, song_id as Song_Id, event_type as Event_Type, timestamp as Timestamp FROM UserPlaybackBehaviour WHERE user_id = @userId");
                playbackBehaviourList = connection.Query<UserPlaybackBehaviour>(queryBuilder.ToString(), new { userId }).ToList();
            }
            return playbackBehaviourList;
        }
    }
}
