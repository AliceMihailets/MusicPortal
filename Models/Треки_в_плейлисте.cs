using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPortal.Models
{
    public class Треки_в_плейлисте
    {
        public Guid ID_Трека { get; set; }
        public Guid ID_Плейлиста { get; set; }
        public Треки трек { get; set; }
        public Плейлист плейлист { get; set; }
    }
}
