using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPortal.Models
{
    public class TracksWithUserPlaylistViewModel
    {
        public TracksWithUserPlaylistViewModel() 
        {
            треки = new List<Треки>();
            Треки_В_Плейлисте = new List<Треки_в_плейлисте>();
        }
        public List<Треки> треки { get; set; }
        public List<Треки_в_плейлисте> Треки_В_Плейлисте { get; set; }
    }
}
