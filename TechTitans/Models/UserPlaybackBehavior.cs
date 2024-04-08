using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTitans.Models
{
    public enum PlaybackEventType
    {
        like = 0,
        start_play = 1,
        end_play = 2,
        dislike = 3,
        skip = 4
    }
    [Table("UserPlaybackBehavior")]
    public class UserPlaybackBehavior
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Key]
        [Column("song_id")]
        public int SongId { get; set; }

        public PlaybackEventType EventType { get; set; }

        public DateTime Timestamp { get; set; }
    }
}

