using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace MusicPortal.Models
{
    public class Жанры
    {
        public Жанры() 
        {
            альбомы = new List<Альбомы>();
        }
        [Key]
        public int ID_жанра { get; set; }
        public string Жанр { get; set; }
        public IEnumerable<Альбомы> альбомы { get; set; }
    }
}
