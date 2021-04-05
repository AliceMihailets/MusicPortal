using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace MusicPortal.Models
{
    public class Альбомы
    {
        public Альбомы() 
        {
            награды = new List<Награды_альбомов>();
            треки = new List<Треки>();
        }
        [Key]
        public Guid ID { get; set; }
        public string Название_альбома { get; set; }
        public int ID_Жанра { get; set; }
        public int ID_Типа_альбома { get; set; }
        public Guid ID_коллектива { get; set; }
        public DateTime Дата_выпуска { get; set; }
        public Жанры жанр { get; set; }
        public Типы_альбомов тип_альбома { get; set; }
        public Коллективы коллектив { get; set; }
        public bool удален { get; set; }
        public IEnumerable<Награды_альбомов> награды { get; set; }
        public IEnumerable<Треки> треки { get; set; }
    }
}
