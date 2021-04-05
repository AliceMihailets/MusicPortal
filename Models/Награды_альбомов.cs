using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPortal.Models
{
    public class Награды_альбомов
    {
        public DateTime Год_получения { get; set; }
        public int ID_награды { get; set; }
        public Guid ID_альбома { get; set; }
        public Награды Наград { get; set; }
        public Альбомы Альбом { get; set; }
    }
}
