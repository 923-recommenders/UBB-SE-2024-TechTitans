using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTitans.Repositories;
using TechTitans.Models;

namespace TechTitans.Test.ServicesTest.Mocks
{
    internal class SongBasicDetailsRepositoryMock : RepositoryMock<SongDataBaseModel>
    {
        public SongBasicInformation SongBasicDetailsToSongBasicInfo_ValidEntity_ReturnsSongBasicInfo(SongDataBaseModel songBasicDetails)
        {
            string artistName = "MockArtistName";
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

        public SongDataBaseModel GetSongBasicDetails_ValidEntity_ReturnsSongBasicDetails(int songId)
        {
            return new SongDataBaseModel
            {
                Song_Id = songId,
                Name = "MockSongName",
                Genre = "MockGenre",
                Subgenre = "MockSubgenre",
                Artist_Id = 1,
                Language = "MockLanguage",
                Country = "MockCountry",
                Album = "MockAlbum",
                Image = "MockImage"
            };
        }

        public List<SongDataBaseModel> GetTop5MostListenedSongs_ValidEntity_ReturnsTop5Songs(int userId)
        {
            return new List<SongDataBaseModel>
            {
                new SongDataBaseModel
                {
                    Song_Id = 1,
                    Name = "MockSongName1",
                    Genre = "MockGenre1",
                    Subgenre = "MockSubgenre1",
                    Artist_Id = 1,
                    Language = "MockLanguage1",
                    Country = "MockCountry1",
                    Album = "MockAlbum1",
                    Image = "MockImage1"
                },
                new SongDataBaseModel
                {
                    Song_Id = 2,
                    Name = "MockSongName2",
                    Genre = "MockGenre2",
                    Subgenre = "MockSubgenre2",
                    Artist_Id = 1,
                    Language = "MockLanguage2",
                    Country = "MockCountry2",
                    Album = "MockAlbum2",
                    Image = "MockImage2"
                },
                new SongDataBaseModel
                {
                    Song_Id = 3,
                    Name = "MockSongName3",
                    Genre = "MockGenre3",
                    Subgenre = "MockSubgenre3",
                    Artist_Id = 1,
                    Language = "MockLanguage3",
                    Country = "MockCountry3",
                    Album = "MockAlbum3",
                    Image = "MockImage3"
                },
                new SongDataBaseModel
                {
                    Song_Id = 4,
                    Name = "MockSongName4",
                    Genre = "MockGenre4",
                    Subgenre = "MockSubgenre4",
                    Artist_Id = 1,
                    Language = "MockLanguage4",
                    Country = "MockCountry4",
                    Album = "MockAlbum4",
                    Image = "MockImage4"
                },
                new SongDataBaseModel
                {
                    Song_Id = 5,
                    Name = "MockSongName5",
                    Genre = "MockGenre5",
                    Subgenre = "MockSubgenre5",
                    Artist_Id = 1,
                    Language = "MockLanguage5",
                    Country = "MockCountry5",
                    Album = "MockAlbum5",
                    Image = "MockImage5"
                }
            };
        }

        public Tuple<SongDataBaseModel, decimal> GetMostPlayedSongPercentile_UserExists_ReturnsSongAndPercentile(int userId)
        {
            return new Tuple<SongDataBaseModel, decimal>(new SongDataBaseModel
            {
                Song_Id = 1,
                Name = "MockSongName",
                Genre = "MockGenre",
                Subgenre = "MockSubgenre",
                Artist_Id = 1,
                Language = "MockLanguage",
                Country = "MockCountry",
                Album = "MockAlbum",
                Image = "MockImage"
            }, 0.5m);
        }

        private SongDataBaseModel GetMostPlayedSong_UserExists_ReturnsSong(int userId)
        {
            return new SongDataBaseModel
            {
                Song_Id = 1,
                Name = "MockSongName",
                Genre = "MockGenre",
                Subgenre = "MockSubgenre",
                Artist_Id = 1,
                Language = "MockLanguage",
                Country = "MockCountry",
                Album = "MockAlbum",
                Image = "MockImage"
            };
        }

        private int GetTotalSongsPlayedByUser_UserExists_ReturnsNonNegativeInteger(int userId)
        {
            return 10;
        }

        private int GetMostListenedSongCount_UserExists_ReturnsNonNegativeInteger(int userId)
        {
            return 5;
        }

        public Tuple<string, decimal> GetMostPlayedArtistPercentile_UserExists_ReturnsArtistAndPercentile(int userId)
        {
            return new Tuple<string, decimal>("MockArtistName", 0.5m);
        }

        public List<string> GetTop5Genres_UserExists_ReturnsFiveStrings(int userId)
        {
            return new List<string>
            {
                "MockGenre1",
                "MockGenre2",
                "MockGenre3",
                "MockGenre4",
                "MockGenre5"
            };
        }

        public List<string> GetAllNewGenresDiscovered_UserExists_ReturnsListOfStrings(int userId)
        {
            return new List<string>
            {
                "MockGenre1",
                "MockGenre2",
                "MockGenre3",
                "MockGenre4",
                "MockGenre5",
                "MockGenre6",
                "MockGenre7"
            };
        }
    }
}
