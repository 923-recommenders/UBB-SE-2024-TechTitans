using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTitans.Models;

namespace TechTitans.Repositories
{
    internal class UserPlaybackBehaviourRepository : Repository<UserPlaybackBehaviour>
    {
        public UserPlaybackBehaviour GetUserPlaybackBehaviour(int userId, int? songId = null, DateTime? timestamp = null)
        {
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
            return _connection.Query<UserPlaybackBehaviour>(queryBuilder.ToString(), new { userId, songId, timestamp }).FirstOrDefault();
        }
        public List<UserPlaybackBehaviour> GetUserPlaybackBehaviour(int userId)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT user_id as User_Id, song_id as Song_Id, event_type as Event_Type, timestamp as Timestamp FROM UserPlaybackBehaviour WHERE user_id = @userId");
            return _connection.Query<UserPlaybackBehaviour>(queryBuilder.ToString(), new { userId }).ToList();
        }
    }
}
