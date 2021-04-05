using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace MusicPortal.Models
{
    public class Коллективы
    {
        public Коллективы() 
        {
            альбомы = new List<Альбомы>();
            исполнители = new List<Исполнители_в_коллективах>();
            премии = new List<Премии_коллективов>();
        }
        [Key]
        public Guid ID { get; set; }
        public string Название_коллектива { get; set; }
        public bool удален { get; set; }
        public DateTime Дата_образования { get; set; }
        public DateTime? Дата_расспада { get; set; }
        public int ID_типа_коллектива { get; set; }
        public IEnumerable<Альбомы> альбомы { get; set; }
        public IEnumerable<Исполнители_в_коллективах> исполнители { get; set; }
        public IEnumerable<Премии_коллективов> премии { get; set; }
        public Типы_коллективов тип_Коллектива { get; set; }
    }
}
