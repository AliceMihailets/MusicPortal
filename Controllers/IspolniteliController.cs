using System;
using System.Collections.Generic;
using System.Linq;
using MusicPortal.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

namespace MusicPortal.Controllers
{
    public class IspolniteliController : Controller
    {
        private musicalportalcontext _context;
        public IspolniteliController(musicalportalcontext context)
        {
            this._context = context;
        }
        public IActionResult Index()
        {
            List<Исполнители> исполнители = _context.Исполнители.Where(t=>t.удален == false).ToList();
            List<IspolniteliWithBandsViewModel> ispolniteli = new List<IspolniteliWithBandsViewModel>();
            foreach (var item in исполнители)
            {
                var tmpsrc = ASCIIEncoding.ASCII.GetBytes(item.Фамилия);
                var tmphash = new MD5CryptoServiceProvider().ComputeHash(tmpsrc);
                Int32 int_hash = BitConverter.ToInt32(tmphash) % 10;
                Исполнители_в_коллективах роль = (from роли in _context.Исполнители_в_коллективах.Include(t=>t.роли_В_Коллективах)
                                                  join исполнитель in _context.Исполнители
                                                  on роли.ID_исполнителя equals исполнитель.ID_исполнителя
                                                  where исполнитель.ID_исполнителя == item.ID_исполнителя
                                                  && исполнитель.удален == false
                                                  select роли).First();
                Коллективы коллектив = (from коллективы in _context.Коллективы.Include(t => t.тип_Коллектива)
                                        join роли in _context.Исполнители_в_коллективах
                                        on коллективы.ID equals роли.ID_коллектива
                                        join исполнитель in _context.Исполнители
                                        on роли.ID_исполнителя equals исполнитель.ID_исполнителя
                                        where исполнитель.ID_исполнителя == item.ID_исполнителя
                                        && исполнитель.удален == false
                                        select коллективы).Single();
                List<Альбомы> альбомы = (from альбом in _context.Альбомы
                                         join коллективы in _context.Коллективы
                                         on альбом.ID_коллектива equals коллективы.ID
                                         join роли in _context.Исполнители_в_коллективах
                                         on коллективы.ID equals роли.ID_коллектива
                                         join иполнитель in _context.Исполнители
                                         on роли.ID_исполнителя equals иполнитель.ID_исполнителя
                                         where иполнитель.ID_исполнителя == item.ID_исполнителя
                                         && альбом.удален == false
                                         select альбом).ToList();
                var ispolniteli_hernya = (from треки in _context.Треки
                                          join альбом in _context.Альбомы
                                          on треки.ID_альбома equals альбом.ID
                                          join коллективы in _context.Коллективы
                                          on альбом.ID_коллектива equals коллективы.ID
                                          join роли in _context.Исполнители_в_коллективах
                                          on коллективы.ID equals роли.ID_коллектива
                                          where роли.ID_исполнителя == item.ID_исполнителя && треки.удалён == false
                                          select new { треки.ID,треки.ID_альбома,треки.Название_трека }).ToList();
                List<Треки> трек = new List<Треки>();
                foreach (var itemitem in ispolniteli_hernya) 
                {
                    трек.Add(new Треки()
                    {
                        ID = itemitem.ID,
                        ID_альбома = itemitem.ID_альбома,
                        Название_трека = itemitem.Название_трека
                    });
                }
                ispolniteli.Add(new IspolniteliWithBandsViewModel()
                {
                    исполнитель = item,
                    роль = роль,
                    коллективы = коллектив,
                    альбомы = альбомы,
                    треки = трек,
                    pic_num = Math.Abs(int_hash) + 1
                });
            }
            return View(ispolniteli);
        }
        public IActionResult FilterIspolnitel(string? pattern)
        {
            IQueryable<Исполнители> ispolQuer = _context.Исполнители.Where(t=>t.удален == false);
            if (pattern != null)
            {
                ispolQuer = ispolQuer.Where(t => EF.Functions.Like(t.Фамилия, pattern + "%") || EF.Functions.Like(t.Имя, pattern + "%"));
            }
            List<Исполнители> исполнители = ispolQuer.ToList();
            List<IspolniteliWithBandsViewModel> ispolniteli = new List<IspolniteliWithBandsViewModel>();
            foreach (var item in исполнители)
            {
                var tmpsrc = ASCIIEncoding.ASCII.GetBytes(item.Фамилия);
                var tmphash = new MD5CryptoServiceProvider().ComputeHash(tmpsrc);
                Int32 int_hash = BitConverter.ToInt32(tmphash) % 10;
                Исполнители_в_коллективах роль = (from роли in _context.Исполнители_в_коллективах.Include(t => t.роли_В_Коллективах)
                                                  join исполнитель in _context.Исполнители
                                                  on роли.ID_исполнителя equals исполнитель.ID_исполнителя
                                                  where исполнитель.ID_исполнителя == item.ID_исполнителя
                                                  select роли).First();
                Коллективы коллектив = (from коллективы in _context.Коллективы.Include(t => t.тип_Коллектива)
                                        join роли in _context.Исполнители_в_коллективах
                                        on коллективы.ID equals роли.ID_коллектива
                                        join исполнитель in _context.Исполнители
                                        on роли.ID_исполнителя equals исполнитель.ID_исполнителя
                                        where исполнитель.ID_исполнителя == item.ID_исполнителя
                                        select коллективы).Single();
                List<Альбомы> альбомы = (from альбом in _context.Альбомы
                                         join коллективы in _context.Коллективы
                                         on альбом.ID_коллектива equals коллективы.ID
                                         join роли in _context.Исполнители_в_коллективах
                                         on коллективы.ID equals роли.ID_коллектива
                                         join иполнитель in _context.Исполнители
                                         on роли.ID_исполнителя equals иполнитель.ID_исполнителя
                                         where иполнитель.ID_исполнителя == item.ID_исполнителя
                                         && иполнитель.удален == false
                                         select альбом).ToList();
                var ispolniteli_hernya = (from треки in _context.Треки
                                          join альбом in _context.Альбомы
                                          on треки.ID_альбома equals альбом.ID
                                          join коллективы in _context.Коллективы
                                          on альбом.ID_коллектива equals коллективы.ID
                                          join роли in _context.Исполнители_в_коллективах
                                          on коллективы.ID equals роли.ID_коллектива
                                          where роли.ID_исполнителя == item.ID_исполнителя && треки.удалён == false
                                          select new { треки.ID, треки.ID_альбома, треки.Название_трека }).ToList();
                List<Треки> трек = new List<Треки>();
                foreach (var itemitem in ispolniteli_hernya)
                {
                    трек.Add(new Треки()
                    {
                        ID = itemitem.ID,
                        ID_альбома = itemitem.ID_альбома,
                        Название_трека = itemitem.Название_трека
                    });
                }
                ispolniteli.Add(new IspolniteliWithBandsViewModel()
                {
                    исполнитель = item,
                    роль = роль,
                    коллективы = коллектив,
                    альбомы = альбомы,
                    треки = трек,
                    pic_num = Math.Abs(int_hash) + 1
                });
            }
            return PartialView("_IspolniteliFiltered", ispolniteli);
        }
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var исполнители = _context.Исполнители.FirstOrDefault(t => t.ID_исполнителя == id);
            if (исполнители == null)
            {
                return NotFound();
            }
            IspolniteliWithBandsViewModel ispolnitel;
            var tmpsrc = ASCIIEncoding.ASCII.GetBytes(исполнители.Фамилия);
            var tmphash = new MD5CryptoServiceProvider().ComputeHash(tmpsrc);
            Int32 int_hash = BitConverter.ToInt32(tmphash) % 10;
            Исполнители_в_коллективах роль = (from роли in _context.Исполнители_в_коллективах.Include(t => t.роли_В_Коллективах)
                                              join исполнитель in _context.Исполнители
                                              on роли.ID_исполнителя equals исполнитель.ID_исполнителя
                                              where исполнитель.ID_исполнителя == исполнители.ID_исполнителя
                                              select роли).First();
            Коллективы коллектив = (from коллективы in _context.Коллективы.Include(t => t.тип_Коллектива)
                                    join роли in _context.Исполнители_в_коллективах
                                    on коллективы.ID equals роли.ID_коллектива
                                    join исполнитель in _context.Исполнители
                                    on роли.ID_исполнителя equals исполнитель.ID_исполнителя
                                    where исполнитель.ID_исполнителя == исполнители.ID_исполнителя
                                    select коллективы).Single();
            List<Альбомы> альбомы = (from альбом in _context.Альбомы
                                     join коллективы in _context.Коллективы
                                     on альбом.ID_коллектива equals коллективы.ID
                                     join роли in _context.Исполнители_в_коллективах
                                     on коллективы.ID equals роли.ID_коллектива
                                     join иполнитель in _context.Исполнители
                                     on роли.ID_исполнителя equals иполнитель.ID_исполнителя
                                     where иполнитель.ID_исполнителя == исполнители.ID_исполнителя
                                     && альбом.удален == false
                                     select альбом).ToList();
            var ispolniteli_hernya = (from треки in _context.Треки
                                      join альбом in _context.Альбомы
                                      on треки.ID_альбома equals альбом.ID
                                      join коллективы in _context.Коллективы
                                      on альбом.ID_коллектива equals коллективы.ID
                                      join роли in _context.Исполнители_в_коллективах
                                      on коллективы.ID equals роли.ID_коллектива
                                      where роли.ID_исполнителя == исполнители.ID_исполнителя && треки.удалён == false
                                      select new { треки.ID, треки.ID_альбома, треки.Название_трека }).ToList();
            List<Треки> трек = new List<Треки>();
            foreach (var itemitem in ispolniteli_hernya)
            {
                трек.Add(new Треки()
                {
                    ID = itemitem.ID,
                    ID_альбома = itemitem.ID_альбома,
                    Название_трека = itemitem.Название_трека
                });
            }
            ispolnitel = new IspolniteliWithBandsViewModel()
            {
                исполнитель = исполнители,
                коллективы = коллектив,
                альбомы = альбомы,
                треки = трек,
                роль = роль,
                pic_num = Math.Abs(int_hash) + 1,
            };
            return View(ispolnitel);
        }
        [HttpGet]
        public IActionResult Create()
        {
            List<string> коллективы = _context.Коллективы.Select(t => t.Название_коллектива).ToList();
            List<string> роли = _context.Роли_в_коллективах.Select(t => t.Название_роли).ToList();
            ViewBag.Roles = роли;
            ViewBag.Bands = коллективы;
            return View();
        }
        [HttpPost]
        public IActionResult Create(Исполнители исполнитель, string band, string role, DateTime start_date, DateTime? end_date)
        {
            исполнитель.ID_исполнителя = Guid.NewGuid();
            if (end_date == DateTime.MinValue)
                end_date = null;
            Исполнители_в_коллективах исполнители = new Исполнители_в_коллективах()
            {
                ID_исполнителя = исполнитель.ID_исполнителя,
                ID_роли_в_коллективе = _context.Роли_в_коллективах.FirstOrDefault(t => t.Название_роли == role).ID_роли_в_коллективе,
                ID_коллектива = _context.Коллективы.FirstOrDefault(t => t.Название_коллектива == band).ID,
                Дата_начала_работы_в_коллективе = start_date,
                Дата_окончания_работы_в_коллективе = end_date,
            };
            _context.Add(исполнитель);
            _context.Add(исполнители);
            _context.SaveChanges();
            return Redirect("/Home/Adding");
        }
        [Authorize(Roles = "admin")]
        public IActionResult DeleteIspolnitel(string? id) 
        {
            Исполнители исполнитель = _context.Исполнители.FirstOrDefault(t => t.ID_исполнителя == Guid.Parse(id));
            исполнитель.удален = true;
            _context.SaveChanges();
            return new JsonResult(new
            {
            });
        }
        [Authorize(Roles = "admin")]
        public IActionResult GetBandsAndRoles() 
        {
            List<string> коллективы = _context.Коллективы.Select(t => t.Название_коллектива).ToList();
            List<string> роли = _context.Роли_в_коллективах.Select(t => t.Название_роли).ToList();
            return new JsonResult(new
            {
                roles = роли,
                bands = коллективы,
            });
        }
        [Authorize(Roles = "admin")]
        public IActionResult EditFields(string? id, string? firstName, string? secondName, string? role, string? band, string? start, string? close) 
        {
            Исполнители исполнитель = _context.Исполнители.FirstOrDefault(t => t.ID_исполнителя == Guid.Parse(id));
            Исполнители_в_коллективах исполнительВКоллективе = _context.Исполнители_в_коллективах.FirstOrDefault(t => t.ID_исполнителя == Guid.Parse(id));
            if (firstName != "" && firstName != null) 
            {
                исполнитель.Имя = firstName;
            }
            if (secondName != "" && secondName != null) 
            {
                исполнитель.Фамилия = secondName;
            }
            if (role != "" && role != null) 
            {
                Роли_в_коллективах роль = _context.Роли_в_коллективах.FirstOrDefault(t => t.Название_роли == role);
                исполнительВКоллективе.ID_роли_в_коллективе = роль.ID_роли_в_коллективе;
            }
            if (band != "" && band != null) 
            {
                Коллективы коллектив = _context.Коллективы.FirstOrDefault(t => t.Название_коллектива == band);
                исполнительВКоллективе.ID_коллектива = коллектив.ID;
            }
            if (start != "" && start != null) 
            {
                исполнительВКоллективе.Дата_начала_работы_в_коллективе = DateTime.Parse(start);
            }
            if (close != "" && close != null)
            {
                исполнительВКоллективе.Дата_окончания_работы_в_коллективе = DateTime.Parse(close);
            }
            else 
            {
                исполнительВКоллективе.Дата_окончания_работы_в_коллективе = null;
            }
            if (firstName != "" && secondName != "" && firstName != null & secondName != null) 
            {
                _context.Update(исполнитель);
            }
            if (role != "" && band != "" && role != null && band != null) 
            {
                _context.Update(исполнительВКоллективе);
            }
            _context.SaveChanges();
            return new JsonResult(new
            {
                Имя = исполнитель.Имя,
                Фамилия = исполнитель.Фамилия,
                band = band,
                role = role,
                start = исполнительВКоллективе.Дата_начала_работы_в_коллективе.ToShortDateString(),
                close = close == null ? "Пока что ещё учавствует в коллективе" : DateTime.Parse(close).ToShortDateString()
            }) ;
        }
    }
}
