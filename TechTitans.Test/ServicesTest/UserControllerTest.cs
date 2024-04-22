using TechTitans;
using NUnit.Framework;
using TechTitans.Repositories;
using TechTitans.Services;
using TechTitans.Models;
using System.Collections.Generic;

namespace TechTitans.Test.Services
{
    internal class UserControllerTest
    {
        [TestFixture]
        public class UserControllerTests
        {
            [Test]
            public void GetRecentlyPlayed_ReturnsCorrectNumberOfSongs()
            {
                
                var testSongRepository = new TestUserSongRepository();
                var expectedSongs = GetExpectedSongs();
                var expectedSongInfo = new SongBasicInformation();
                var _testSong = new SongDataBaseModel();

                
                testSongRepository.Setup(expectedSongs, expectedSongInfo, _testSong);

                var _userController = new UserController(testSongRepository);
                var result = _userController.GetRecentlyPlayed();

                Assert.Equals(6, result.Count);
            }

            private List<SongDataBaseModel> GetExpectedSongs()
            {
                return new List<SongDataBaseModel>
                    {
                        new SongDataBaseModel
                        {
                            Song_Id = 1,
                            Name = "Song 1",
                            Genre = "Pop",
                            Subgenre = "Dance",
                            Artist_Id = 101,
                            Language = "English",
                            Country = "USA",
                            Album = "Album 1",
                            Image = "song1_img.png"
                        },
                        new SongDataBaseModel
                        {
                            Song_Id = 2,
                            Name = "Song 2",
                            Genre = "Rock",
                            Subgenre = "Hard Rock",
                            Artist_Id = 102,
                            Language = "English",
                            Country = "Canada",
                            Album = "Album 2",
                            Image = "song2_img.png"
                        },
                        new SongDataBaseModel
                        {
                            Song_Id = 3,
                            Name = "Song 3",
                            Genre = "Jazz",
                            Subgenre = "Smooth Jazz",
                            Artist_Id = 103,
                            Language = "English",
                            Country = "UK",
                            Album = "Album 3",
                            Image = "song3_img.png"
                        },
                        new SongDataBaseModel
                        {
                            Song_Id = 4,
                            Name = "Song 4",
                            Genre = "Classical",
                            Subgenre = "Symphony",
                            Artist_Id = 104,
                            Language = "English",
                            Country = "Germany",
                            Album = "Album 4",
                            Image = "song4_img.png"
                        },
                        new SongDataBaseModel
                        {
                            Song_Id = 5,
                            Name = "Song 5",
                            Genre = "Hip Hop",
                            Subgenre = "Rap",
                            Artist_Id = 105,
                            Language = "English",
                            Country = "USA",
                            Album = "Album 5",
                            Image = "song5_img.png"
                        },
                        new SongDataBaseModel
                        {
                            Song_Id = 6,
                            Name = "Song 6",
                            Genre = "Electronic",
                            Subgenre = "Techno",
                            Artist_Id = 106,
                            Language = "English",
                            Country = "Australia",
                            Album = "Album 6",
                            Image = "song6_img.png"
                        }
                    };

            }
        }

        // Concrete implementation of IUserSongRepository for testing
        private class TestUserSongRepository : IUserSongRepository
        {
            private List<SongDataBaseModel> _songs;
            private SongBasicInformation _songInfo;
            private SongDataBaseModel _testSong;

            public void Setup(List<SongDataBaseModel> songs, SongBasicInformation songInfo, SongDataBaseModel testSong)
            {
                _songs = songs;
                _songInfo = songInfo;
                _testSong = testSong;
            }

            public List<SongDataBaseModel> GetAll()
            {
                return _songs;
            }

            public SongBasicInformation ConvertSongDataBaseModelToSongInfo(SongDataBaseModel song)
            {
                // Assuming this method is supposed to return _songInfo for any input
                // Adjust this logic based on your actual implementation
                return _songInfo;
            }

            public SongDataBaseModel GetById(int id)
            {
                throw new NotImplementedException();
            }

            IEnumerable<SongDataBaseModel> IRepository<SongDataBaseModel>.GetAll()
            {
                throw new NotImplementedException();
            }

            public bool Add(SongDataBaseModel entity)
            {
                throw new NotImplementedException();
            }

            public bool Update(SongDataBaseModel entity)
            {
                throw new NotImplementedException();
            }

            public bool Delete(SongDataBaseModel entity)
            {
                throw new NotImplementedException();
            }
        }
    }
}

