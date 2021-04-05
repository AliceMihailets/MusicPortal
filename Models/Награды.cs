using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace MusicPortal.Models
{
    public class Награды
    {
        public Награды() 
        {
            альбомы = new List<Награды_альбомов>();
        }
        [Key]
        public int ID_награды { get; set; }
        public string Название_награды { get; set; }
        public IEnumerable<Награды_альбомов> альбомы { get; set; }
    }
}
