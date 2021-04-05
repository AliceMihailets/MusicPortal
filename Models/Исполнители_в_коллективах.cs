using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPortal.Models
{
    public class Исполнители_в_коллективах
    {
        public Guid ID_исполнителя { get; set; }
        public Guid ID_коллектива { get; set; }
        public int ID_роли_в_коллективе { get; set; }
        public DateTime Дата_начала_работы_в_коллективе { get; set; }
        public DateTime? Дата_окончания_работы_в_коллективе { get; set; }
        public Роли_в_коллективах роли_В_Коллективах { get; set; }
        public Коллективы коллектив { get; set; }
        public Исполнители исполнитель { get; set; }
    }
}
