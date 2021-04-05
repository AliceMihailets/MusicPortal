using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace MusicPortal.Models
{
    public class Треки
    {
        public Треки() 
        {
            плейлиты = new List<Треки_в_плейлисте>();
        }
        [Key]
        public Guid ID { get; set; }
        public string Название_трека { get; set; }
        public Guid ID_альбома { get; set; }
        public byte[] Данные { get; set; }
        public bool удалён { get; set; }
        public IEnumerable<Треки_в_плейлисте> плейлиты { get; set; }
        public Альбомы Альбом { get; set; }
    }
}
