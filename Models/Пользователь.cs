using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace MusicPortal.Models
{
    public class Пользователь
    {
        public Пользователь()
        {
            плейлисты = new List<Плейлист>();
        }
        [Key]
        public Guid ID { get; set; }
        [Required(ErrorMessage = "Поле должно быть заполненым")]
        public string Никнейм { get; set; }
        [Required(ErrorMessage = "Поле должно быть заполненым")]
        public string Пароль { get; set; }
        public string _salt { get; set; }
        public string _token { get; set; }
        public byte[] аватарка { get; set; }
        public IEnumerable<Плейлист> плейлисты { get; set; }
    }
}
