using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTitans.Models;
using TechTitans.Repositories;
namespace TechTitans.Services
{
    internal class UserService
    {
        private UserSongRepository SongRepo= new UserSongRepository();
       
        public List<SongBasicInformation> GetRecentlyPlayed() {
            List<SongBasicInformation> groupOfSongsInformation=new List<SongBasicInformation>();
            foreach (SongDataBaseModel song in SongRepo.GetAll()){ 
                SongBasicInformation song_info = SongRepo.ConvertSongDataBaseModelToSongInfo(song);
                groupOfSongsInformation.Add(song_info);
            }
            return groupOfSongsInformation.Take(6).ToList();
        }
    }
}
