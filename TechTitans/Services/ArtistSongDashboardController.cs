using TechTitans.Models;
using TechTitans.Repositories;

namespace TechTitans.Services
{
    public class ArtistSongDashboardController
    {
        private Repository<SongDataBaseModel> SongRepository = new Repository<SongDataBaseModel>();
        private Repository<SongFeatures> FeatureRepository = new Repository<SongFeatures>();
        private Repository<SongRecommendationDetails> SongRecommendationRepository = new Repository<SongRecommendationDetails>();
        private Repository<ArtistDetails> ArtistRepository = new Repository<ArtistDetails>();

        public SongBasicInformation TransformSongDataBaseModelToSongInfo(SongDataBaseModel song)
        {
            SongBasicInformation songInfo = new SongBasicInformation();
            songInfo.SongId = song.Song_Id;
            songInfo.Name = song.Name;
            songInfo.Genre = song.Genre;
            songInfo.Subgenre = song.Subgenre;
            songInfo.Language = song.Language;
            songInfo.Country = song.Country;
            songInfo.Album = song.Album;
            songInfo.Image = song.Image;
            foreach (ArtistDetails artist in ArtistRepository.GetAll())
            {
                if (artist.Artist_Id == song.Artist_Id)
                {
                    songInfo.Artist = artist.Name;
                }
            }
            foreach (SongFeatures feature in FeatureRepository.GetAll())
            {
                if (feature.Song_Id == song.Song_Id)
                {
                    songInfo.Features.Add(feature.ToString());
                }
            }
            return songInfo;
        }

        public List<SongBasicInformation> GetAllArtistSongs(int artistId)
        {
            List<SongBasicInformation> artistSongs = new List<SongBasicInformation>();
            foreach (SongDataBaseModel song in SongRepository.GetAll())
            {
                if (song.Artist_Id == artistId)
                {
                    SongBasicInformation songInfo = TransformSongDataBaseModelToSongInfo(song);
                    artistSongs.Add(songInfo);
                }
            }
            return artistSongs;
        }

        public List<SongBasicInformation> searchSongsByTitle(string title)
        {
            List<SongBasicInformation> songs = new List<SongBasicInformation>();
            foreach (SongDataBaseModel song in SongRepository.GetAll())
            {
                if (song.Name.ToLower().Trim().Contains(title.ToLower()))
                {
                    SongBasicInformation songInfo = TransformSongDataBaseModelToSongInfo(song);
                    songs.Add(songInfo);
                }
            }
            return songs;
        }


        public SongBasicInformation GetSongInformation(int songId)
        {
            foreach (SongDataBaseModel song in SongRepository.GetAll())
            {
                if (song.Song_Id == songId)
                {
                    SongBasicInformation songInfo = TransformSongDataBaseModelToSongInfo(song);
                    return songInfo;
                }
            }
            return null;
        }
        
        public SongRecommendationDetails GetSongRecommandationDetails(int songId)
        {
            foreach (SongRecommendationDetails songDetails in SongRecommendationRepository.GetAll())
            {
                if (songDetails.Song_Id == songId)
                {
                    return songDetails;
                }
            }

            return new SongRecommendationDetails();
            
        }
        public ArtistDetails GetArtistInfoBySong(int SongId)
        {
            foreach (SongDataBaseModel song in SongRepository.GetAll())
            {
                if (song.Song_Id == SongId)
                {
                    foreach (ArtistDetails artist in ArtistRepository.GetAll())
                    {
                        if (artist.Artist_Id == song.Artist_Id)
                        {
                            return artist;
                        }
                    }
                }
            }
            return null;
        }

        public ArtistDetails GetMostPublishedArtist()
        {
            Dictionary<int, int> artistCount = new Dictionary<int, int>();
            foreach (SongDataBaseModel song in SongRepository.GetAll())
            {
                if (artistCount.ContainsKey(song.Artist_Id))
                {
                    artistCount[song.Artist_Id]++;
                }
                else
                {
                    artistCount.Add(song.Artist_Id, 1);
                }
            }
            int max = 0;
            int artistId = 0;
            foreach (KeyValuePair<int, int> entry in artistCount)
            {
                if (entry.Value > max)
                {
                    max = entry.Value;
                    artistId = entry.Key;
                }
            }
            foreach (ArtistDetails artist in ArtistRepository.GetAll())
            {
                if (artist.Artist_Id == artistId)
                {
                    return artist;
                }
            }
            return null;
        }
        public List<SongBasicInformation> GetSongsByMostPublishedArtistForMainPage()
        {
            List<SongBasicInformation> songs = new List<SongBasicInformation>();
            int artistId = GetMostPublishedArtist().Artist_Id;
            foreach (SongDataBaseModel song in SongRepository.GetAll())
            {
                if (song.Artist_Id == artistId)
                {
                    SongBasicInformation songInfo = TransformSongDataBaseModelToSongInfo(song);
                    songs.Add(songInfo);
                }
            }
            return songs;
        }   
    }
}
