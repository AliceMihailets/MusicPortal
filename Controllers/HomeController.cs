using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MusicPortal.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace MusicPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private musicalportalcontext _context;

        public HomeController(ILogger<HomeController> logger, musicalportalcontext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            Guid UserId = Guid.Empty;
            List<Треки_в_плейлисте> треки_В_Плейлисте = new List<Треки_в_плейлисте>();
            if (User.Identity.Name != null) 
            {
                UserId = Guid.Parse(User.Identity.Name);
                треки_В_Плейлисте = _context.Треки_в_плейлисте.Include(t => t.плейлист).Where(t => t.плейлист.ID_Пользователя == UserId).ToList();
            }
            List<Коллективы> коллективы = _context.Коллективы.OrderBy(t => t.Дата_образования).Take(6).ToList();
            List<Альбомы> альбомы = _context.Альбомы.Take(6).ToList();
            var index_hernya = _context.Треки.Where(t=>t.удалён==false).Take(30).Select(t=>new { t.ID,t.ID_альбома,t.Название_трека}).ToList();
            List<Треки> треки = new List<Треки>();
            foreach (var item in index_hernya) 
            {
                треки.Add(new Треки()
                {
                    ID = item.ID,
                    ID_альбома = item.ID_альбома,
                    Название_трека = item.Название_трека
                });
            }
            TracksBandsAlbumsViewModel tracksBandsAlbumsViewModel = new TracksBandsAlbumsViewModel()
            {
                альбомы = альбомы,
                треки = треки,
                коллективы = коллективы,
                треки_В_Плейлисте = треки_В_Плейлисте
            };
            return View(tracksBandsAlbumsViewModel);
        }
        [Authorize(Roles = "admin")]
        public IActionResult Adding() 
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
