using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTitans.Models;
using TechTitans.Repositories;
namespace TechTitans.Services
{
    internal class UserService
    {
        private UserRepository SongRepo= new UserRepository();
        
        public List<SongBasicInformation> get_recently_played() {
            List<SongBasicInformation> list_song=new List<SongBasicInformation>();
            foreach (SongDataBaseModel song in SongRepo.GetAll()){ 
                SongBasicInformation song_info = SongRepo.SongBasicDetailsToSongBasicInfo(song);
                list_song.Add(song_info);
            }
            return list_song.Take(6).ToList();
        }
    }
}
