using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPortal.Models
{
    public class IspolniteliWithBandsViewModel
    {
        public IspolniteliWithBandsViewModel() 
        {
            альбомы = new List<Альбомы>();
            треки = new List<Треки>();
        }
        public Исполнители исполнитель { get; set; }
        public Коллективы коллективы { get; set; }
        public Исполнители_в_коллективах роль { get; set; }
        public List<Альбомы> альбомы { get; set; }
        public List<Треки> треки { get; set; }
        public int pic_num { get; set; }
    }
}
