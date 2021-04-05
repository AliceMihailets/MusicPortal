using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPortal.Models
{
    public class Премии_коллективов
    {
        public DateTime Год_вручения { get; set; }
        public Guid ID_премии { get; set; }
        public Guid ID_коллектива { get; set; }
        public Коллективы коллектив { get; set; }
        public Премии премия { get; set; }
    }
}
