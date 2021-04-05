using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPortal.Models
{
    public class AlbumsWithTreksViewModel
    {
        public AlbumsWithTreksViewModel() 
        {
            треки = new List<Треки>();
        }
        public Альбомы альбом { get; set; }
        public IEnumerable<Треки> треки { get; set; }
        public int pic_num { get; set; }
    }
}
