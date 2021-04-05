using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPortal.Models
{
    public class BandsWithTracksViewModel
    {
        public BandsWithTracksViewModel()
        {
            треки = new List<Треки>();
            альбомы = new List<Альбомы>();
            исполнители = new List<Исполнители>();
        }
        public int pic_num { get; set; }
        public Коллективы коллектив { get; set; }
        public List<Альбомы> альбомы { get; set; }
        public List<Треки> треки { get; set; }
        public List<Исполнители> исполнители { get; set; }
    }
}
