using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace MusicPortal.Models
{
    public class Премии
    {
        public Премии() 
        {
            коллективы = new List<Премии_коллективов>();
        }
        [Key]
        public Guid ID { get; set; }
        public string Название_премии { get; set; }
        public IEnumerable<Премии_коллективов> коллективы { get; set; }
    }
}
