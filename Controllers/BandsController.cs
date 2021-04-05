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
    public class BandsController : Controller
    {
        private musicalportalcontext _context;
        public BandsController(musicalportalcontext context) 
        {
            this._context = context;
        }
        public IActionResult Index()
        {
            List<Коллективы> коллективы = _context.Коллективы.Include(t => t.тип_Коллектива).Where(t=>t.удален == false).ToList();
            List<BandsWithTracksViewModel> bands = new List<BandsWithTracksViewModel>();
            foreach (var item in коллективы) 
            {
                var tmpsrc = ASCIIEncoding.ASCII.GetBytes(item.Название_коллектива);
                var tmphash = new MD5CryptoServiceProvider().ComputeHash(tmpsrc);
                Int32 int_hash = BitConverter.ToInt32(tmphash) % 10;
                List<Альбомы> альбомы = (from альбом in _context.Альбомы
                                         where альбом.ID_коллектива == item.ID
                                         && альбом.удален == false
                                         select альбом).ToList();
                var bands_hernya = (from трек in _context.Треки
                                    join альбом in _context.Альбомы
                                    on трек.ID_альбома equals альбом.ID
                                    where альбом.ID_коллектива == item.ID && трек.удалён == false
                                    select new 
                                    {
                                        трек.ID,
                                        трек.ID_альбома,
                                        трек.Название_трека
                                    }).ToList();
                List<Треки> треки = new List<Треки>();
                foreach (var itemitem in bands_hernya) 
                {
                    треки.Add(new Треки()
                    {
                        ID = itemitem.ID,
                        ID_альбома = itemitem.ID_альбома,
                        Название_трека = itemitem.Название_трека
                    });
                }
                bands.Add(new BandsWithTracksViewModel()
                {
                    коллектив = item,
                    альбомы = альбомы,
                    треки = треки,
                    pic_num = Math.Abs(int_hash) + 1
                });
            }
            return View(bands);
        }
        public IActionResult FilterBand(string? pattern)
        {
            IQueryable<Коллективы> bandQuer = _context.Коллективы.Include(t => t.тип_Коллектива).Where(t=>t.удален == false);
            if (pattern != null)
            {
                bandQuer = bandQuer.Where(t => EF.Functions.Like(t.Название_коллектива, pattern + "%"));
            }
            List<Коллективы> bandsQ = bandQuer.ToList();
            List<BandsWithTracksViewModel> bands = new List<BandsWithTracksViewModel>();
            foreach (var item in bandsQ)
            {
                var tmpsrc = ASCIIEncoding.ASCII.GetBytes(item.Название_коллектива);
                var tmphash = new MD5CryptoServiceProvider().ComputeHash(tmpsrc);
                Int32 int_hash = BitConverter.ToInt32(tmphash) % 10;
                List<Альбомы> альбомы = (from альбом in _context.Альбомы
                                         where альбом.ID_коллектива == item.ID
                                         && альбом.удален == false
                                         select альбом).ToList();
                var bands_hernya = (from трек in _context.Треки
                                    join альбом in _context.Альбомы
                                    on трек.ID_альбома equals альбом.ID
                                    where альбом.ID_коллектива == item.ID && трек.удалён == false
                                    select new
                                    {
                                        трек.ID,
                                        трек.ID_альбома,
                                        трек.Название_трека
                                    }).ToList();
                List<Треки> треки = new List<Треки>();
                foreach (var itemitem in bands_hernya)
                {
                    треки.Add(new Треки()
                    {
                        ID = itemitem.ID,
                        ID_альбома = itemitem.ID_альбома,
                        Название_трека = itemitem.Название_трека
                    });
                }
                bands.Add(new BandsWithTracksViewModel()
                {
                    коллектив = item,
                    альбомы = альбомы,
                    треки = треки,
                    pic_num = Math.Abs(int_hash) + 1
                });
            }
            return PartialView("_BandsFiltered", bands);
        }
        public IActionResult Details(Guid? id) 
        {
            if (id == null)
            {
                return NotFound();
            }
            var коллективы = _context.Коллективы
                .Include(t => t.тип_Коллектива)
                .Include(t=>t.исполнители)
                .FirstOrDefault(t => t.ID == id);
            if (коллективы == null)
            {
                return NotFound();
            }
            BandsWithTracksViewModel bands;
            var tmpsrc = ASCIIEncoding.ASCII.GetBytes(коллективы.Название_коллектива);
            var tmphash = new MD5CryptoServiceProvider().ComputeHash(tmpsrc);
            Int32 int_hash = BitConverter.ToInt32(tmphash) % 10;
            List<Альбомы> альбомы = (from альбом in _context.Альбомы
                                     where альбом.ID_коллектива == коллективы.ID
                                     where альбом.удален == false
                                     select альбом).ToList();
            var bands_hernya = (from трек in _context.Треки
                                join альбом in _context.Альбомы
                                on трек.ID_альбома equals альбом.ID
                                where альбом.ID_коллектива == коллективы.ID && трек.удалён == false
                                select new
                                {
                                    трек.ID,
                                    трек.ID_альбома,
                                    трек.Название_трека
                                }).ToList();
            List<Треки> треки = new List<Треки>();
            foreach (var itemitem in bands_hernya)
            {
                треки.Add(new Треки()
                {
                    ID = itemitem.ID,
                    ID_альбома = itemitem.ID_альбома,
                    Название_трека = itemitem.Название_трека
                });
            }
            List<Исполнители> исполнители = (from исполнитель in _context.Исполнители
                                             join исполнительВКоллективе in _context.Исполнители_в_коллективах
                                             on исполнитель.ID_исполнителя equals исполнительВКоллективе.ID_исполнителя
                                             where исполнительВКоллективе.ID_коллектива == коллективы.ID
                                             && исполнитель.удален == false
                                             select исполнитель).ToList();
            bands = new BandsWithTracksViewModel()
            {
                коллектив = коллективы,
                альбомы = альбомы,
                треки = треки,
                pic_num = Math.Abs(int_hash) + 1,
                исполнители = исполнители
            };
            return View(bands);
        }
        public IActionResult DeleteBand(string? id) 
        {
            Коллективы коллектив = _context.Коллективы.FirstOrDefault(t => t.ID == Guid.Parse(id));
            коллектив.удален = true;
            _context.SaveChanges();
            return new JsonResult(new
            {
            });
        }
        [Authorize(Roles = "admin")]
        public IActionResult GetTypes() 
        {
            List<string> типы = _context.Типы_коллективов.Select(t => t.Тип_коллектива).ToList();
            return new JsonResult(new
            {
                type = типы,
            });
        }
        [Authorize(Roles = "admin")]
        public IActionResult EditFields(string? id, string? name, string? type, string? start, string? close) 
        {
            Коллективы коллектив = _context.Коллективы.FirstOrDefault(t => t.ID == Guid.Parse(id));
            if (name != "" && name != null)
                коллектив.Название_коллектива = name;
            if (type != "" && type != null)
                коллектив.ID_типа_коллектива = _context.Типы_коллективов.FirstOrDefault(t => t.Тип_коллектива == type).ID_типа_коллектива;
            if (start != "" && start != null && start!="undefined")
                коллектив.Дата_образования = DateTime.Parse(start);
            if (close != "" && close != null && close != "undefined")
                коллектив.Дата_расспада = DateTime.Parse(close);
            else
                коллектив.Дата_расспада = null;
            _context.Update(коллектив);
            _context.SaveChanges();
            return new JsonResult(new
            {
                name = коллектив.Название_коллектива,
                type = _context.Коллективы.Include(t=>t.тип_Коллектива).FirstOrDefault(t=>t.ID == коллектив.ID).тип_Коллектива.Тип_коллектива,
                start = коллектив.Дата_образования.ToShortDateString(),
                close = коллектив.Дата_расспада == null ? "Ещё не расспался суки" : коллектив.Дата_расспада?.ToShortDateString(),
            });
        }
        [HttpGet]
        public IActionResult Create() 
        {
            List<string> типы = _context.Типы_коллективов.OrderBy(t => t.ID_типа_коллектива).Select(t => t.Тип_коллектива).ToList();
            ViewBag.types = типы;
            ViewBag.typesCount = типы.Count();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Коллективы коллектив) 
        {
            коллектив.ID_типа_коллектива++;
            _context.Add(коллектив);
            _context.SaveChanges();
            return Redirect("/Home/Adding");
        }
    }
}
