using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTitans.Repositories;
using TechTitans.Models;
using Dapper;

namespace TechTitans.Repositories
{
    internal class UserRepository : Repository<SongDataBaseModel>
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
        }
    }

