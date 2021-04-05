using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace MusicPortal.Models
{
    public class Плейлист
    {
        public Плейлист() 
        {
            треки = new List<Треки_в_плейлисте>();
        }
        [Key]
        public Guid ID { get; set; }
        public Guid ID_Пользователя { get; set; }
        public string Название { get; set; }
        public string Описание { get; set; }
        public List<Треки_в_плейлисте> треки { get; set; }
        public Пользователь Пользователь { get; set; }
    }
}
