using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace MusicPortal.Models
{
    public class Типы_альбомов
    {
        public Типы_альбомов() 
        {
            альбомы = new List<Альбомы>();
        }
        [Key]
        public int ID_типа_альбомы { get; set; }
        public string Тип_альбома { get; set; }
        public IEnumerable<Альбомы> альбомы { get; set; }
    }
}
