using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPortal.Models
{
    public class TracksBandsAlbumsViewModel
    {
        public TracksBandsAlbumsViewModel()
        {
            треки = new List<Треки>();
            альбомы = new List<Альбомы>();
            коллективы = new List<Коллективы>();
            треки_В_Плейлисте = new List<Треки_в_плейлисте>();
        }
        public List<Треки> треки { get; set; }
        public List<Альбомы> альбомы { get; set; }
        public List<Коллективы> коллективы { get; set; }
        public List<Треки_в_плейлисте> треки_В_Плейлисте { get; set; }
    }
}
