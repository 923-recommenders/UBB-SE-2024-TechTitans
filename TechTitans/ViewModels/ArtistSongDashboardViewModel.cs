using System.ComponentModel;
using TechTitans.Enums;
using TechTitans.Models;

namespace TechTitans.ViewModels
{
    class ArtistSongDashboardViewModel
    {
        public SongBasicInformation SongInfo { get; set; }
        public AuthorDetails ArtistInfo { get; set; }
        public SongRecommendationDetails SongDetails { get; set; }
    }
}
