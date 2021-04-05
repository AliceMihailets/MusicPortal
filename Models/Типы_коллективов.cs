using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace MusicPortal.Models
{
    public class Типы_коллективов
    {
        public Типы_коллективов() 
        {
            коллективы = new List<Коллективы>();
        }
        [Key]
        public int ID_типа_коллектива { get; set; }
        public string Тип_коллектива { get; set; }
        public IEnumerable<Коллективы> коллективы { get; set; }
    }
}
