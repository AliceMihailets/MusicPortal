using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace MusicPortal.Models
{
    public class Роли_в_коллективах
    {
        public Роли_в_коллективах() 
        {
            исполнители = new List<Исполнители_в_коллективах>();
        }
        [Key]
        public int ID_роли_в_коллективе { get; set; }
        public string Название_роли { get; set; }
        public IEnumerable<Исполнители_в_коллективах> исполнители { get; set; }
    }
}
