using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace MusicPortal.Models
{
    public class Исполнители
    {
        public Исполнители() 
        {
            коллективы = new List<Исполнители_в_коллективах>();
        }
        [Key]
        public Guid ID_исполнителя { get; set; }
        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public bool удален { get; set; }
        public IEnumerable<Исполнители_в_коллективах> коллективы { get; set; }
    }
}
