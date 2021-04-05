using System;
using System.Collections.Generic;
using System.Linq;
using MusicPortal.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace MusicPortal.Controllers
{
    public class TracksController : Controller
    {
        private musicalportalcontext _context;
        public TracksController(musicalportalcontext context)
        {
            _context = context;
        }
        public IActionResult AddTrack()
        {
            List<string> исполнители = _context.Коллективы.Select(t => t.Название_коллектива).ToList();
            ViewBag.исполнители = исполнители;
            return View();
        }
        public IActionResult GetAlbums(string? band)
        {
            List<Альбомы> альбомы = (from альбом in _context.Альбомы
                                     join коллектив in _context.Коллективы
                                     on альбом.ID_коллектива equals коллектив.ID
                                     where коллектив.Название_коллектива == band
                                     select альбом).ToList();
            JsonResult json = Json(альбомы);
            return json;
        }
        [HttpPost]
        public IActionResult AddTrack(Треки трек, IFormFile data) 
        {
            if (data != null) 
            {
                byte[] audiodata = null;
                using (var binaryReader = new BinaryReader(data.OpenReadStream())) 
                {
                    audiodata = binaryReader.ReadBytes((int)data.Length);
                }
                трек.Данные = audiodata;
            }
            _context.Add(трек);
            _context.SaveChanges();
            return Redirect("/Home/Adding");
        }
        [HttpGet]
        public IActionResult Index() 
        {
            List<Треки_в_плейлисте> треки_В_Плейлисте = new List<Треки_в_плейлисте>();
            if (User.Identity.Name != null) 
            {
                Guid UserId = Guid.Parse(User.Identity.Name);
                треки_В_Плейлисте = (from trak in _context.Треки_в_плейлисте
                                     join playlist in _context.Плейлист
                                     on trak.ID_Плейлиста equals playlist.ID
                                     where playlist.ID_Пользователя == UserId
                                     select trak).ToList();
            }
            List<string> жанры = _context.Жанры.Select(t => t.Жанр).ToList();
            List<string> группы = _context.Коллективы.Select(t => t.Название_коллектива).ToList();
            ViewBag.Genres = жанры;
            ViewBag.Bands = группы;
            var hernya = _context.Треки.Select(t => new { t.ID, t.ID_альбома, t.Название_трека }).ToList();
            List<Треки> треки = new List<Треки>();
            foreach (var item in hernya) 
            {
                треки.Add(new Треки()
                {
                    ID = item.ID,
                    ID_альбома = item.ID_альбома,
                    Название_трека = item.Название_трека
                });
            }
            TracksWithUserPlaylistViewModel tracks = new TracksWithUserPlaylistViewModel()
            {
                треки = треки,
                Треки_В_Плейлисте = треки_В_Плейлисте
            };
            return View(tracks);
        }
        [Authorize(Roles = "user")]
        public IActionResult UserPlaylist()
        {
            List<string> жанры = _context.Жанры.Select(t=>t.Жанр).ToList();
            List<string> группы = _context.Коллективы.Select(t => t.Название_коллектива).ToList();
            ViewBag.Genres = жанры;
            ViewBag.Bands = группы;
            Guid UserId = Guid.Parse(User.Identity.Name);
            var hernya = (from плейлист in _context.Плейлист
                          join треквплейлисте in _context.Треки_в_плейлисте
                          on плейлист.ID equals треквплейлисте.ID_Плейлиста
                          join трек in _context.Треки
                          on треквплейлисте.ID_Трека equals трек.ID
                          where плейлист.ID_Пользователя == UserId
                          select new 
                          {
                              трек.ID,
                              трек.ID_альбома,
                              трек.Название_трека,
                          }).ToList();
            List<Треки> треки = new List<Треки>();
            foreach (var item in hernya) 
            {
                треки.Add(new Треки()
                {
                    ID = item.ID,
                    ID_альбома = item.ID_альбома,
                    Название_трека = item.Название_трека
                });
            }
            var треки_В_Плейлисте = (from trak in _context.Треки_в_плейлисте
                                     join playlist in _context.Плейлист
                                     on trak.ID_Плейлиста equals playlist.ID
                                     where playlist.ID_Пользователя == UserId
                                     select trak).ToList();
            TracksWithUserPlaylistViewModel tracks = new TracksWithUserPlaylistViewModel()
            {
                треки = треки,
                Треки_В_Плейлисте = треки_В_Плейлисте
            };
            return View(tracks);
        }
        public string GetSongData(string? id) 
        {
            string data = "";
            byte[] byte_data = _context.Треки.FirstOrDefault(t => t.ID == Guid.Parse(id)).Данные;
            data = Convert.ToBase64String(byte_data);
            return data;
        }
        public IActionResult FilterTracks(string? pattern, string? genre, string? band, string? is_all) 
        {
            List<Треки> треки = new List<Треки>();
            List<Треки_в_плейлисте> треки_В_Плейлисте = new List<Треки_в_плейлисте>();
            Guid UserId = Guid.Empty;
            if (User.Identity.IsAuthenticated)
                UserId = Guid.Parse(User.Identity.Name);
            if (is_all=="false")
            {
                IQueryable<Треки> tracksq = from плейлист in _context.Плейлист
                                            join треквплейлисте in _context.Треки_в_плейлисте
                                            on плейлист.ID equals треквплейлисте.ID_Плейлиста
                                            join трек in _context.Треки
                                            on треквплейлисте.ID_Трека equals трек.ID
                                            where плейлист.ID_Пользователя == UserId
                                            select трек;
                if (pattern != null && pattern != "") 
                {
                    tracksq = tracksq.Where(t => EF.Functions.Like(t.Название_трека, pattern + "%"));
                }
                if (genre != null && pattern != "") 
                {
                    tracksq = from trak in tracksq
                              join album in _context.Альбомы.Include(t => t.жанр)
                              on trak.ID_альбома equals album.ID
                              where album.жанр.Жанр == genre
                              select trak;
                }
                if (band != null && pattern != "") 
                {
                    tracksq = from trak in tracksq
                              join album in _context.Альбомы.Include(t => t.коллектив)
                              on trak.ID_альбома equals album.ID
                              where album.коллектив.Название_коллектива == band
                              select trak;
                }
                var hernya = tracksq.Select(t => new { t.ID, t.ID_альбома, t.Название_трека }).ToList();
                foreach (var item in hernya)
                {
                    треки.Add(new Треки()
                    {
                        ID = item.ID,
                        ID_альбома = item.ID_альбома,
                        Название_трека = item.Название_трека
                    });
                }
                треки_В_Плейлисте = (from trak in _context.Треки_в_плейлисте
                                         join playlist in _context.Плейлист
                                         on trak.ID_Плейлиста equals playlist.ID
                                         where playlist.ID_Пользователя == UserId
                                         select trak).ToList();
            }
            else 
            {
                IQueryable<Треки> tracksq = _context.Треки;
                if (pattern != null && pattern != "")
                {
                    tracksq = tracksq.Where(t => EF.Functions.Like(t.Название_трека, pattern + "%"));
                }
                if (genre != null && genre != "")
                {
                    tracksq = from trak in tracksq
                              join album in _context.Альбомы.Include(t => t.жанр)
                              on trak.ID_альбома equals album.ID
                              where album.жанр.Жанр == genre
                              select trak;
                }
                if (band != null && band != "") 
                {
                    tracksq = from trak in tracksq
                              join album in _context.Альбомы.Include(t => t.коллектив)
                              on trak.ID_альбома equals album.ID
                              where album.коллектив.Название_коллектива == band
                              select trak;
                }
                var hernya = tracksq.ToList();
                foreach (var item in hernya) 
                {
                    треки.Add(new Треки()
                    {
                        ID = item.ID,
                        ID_альбома = item.ID_альбома,
                        Название_трека = item.Название_трека
                    });
                }
                треки_В_Плейлисте = (from trak in _context.Треки_в_плейлисте
                                     join playlist in _context.Плейлист
                                     on trak.ID_Плейлиста equals playlist.ID
                                     where playlist.ID_Пользователя == UserId
                                     select trak).ToList();
            }
            TracksWithUserPlaylistViewModel tracks = new TracksWithUserPlaylistViewModel()
            {
                треки = треки,
                Треки_В_Плейлисте = треки_В_Плейлисте
            };
            return PartialView("_TracksFiltered", tracks);
        }
        [Authorize(Roles = "user")]
        public IActionResult AddSong(string? id) 
        {
            if (id == null || id == "") 
            {
                return new JsonResult(new
                {
                    error = "err"
                });
            }
            Guid UserId = Guid.Parse(User.Identity.Name);
            Треки трек = _context.Треки.FirstOrDefault(t => t.ID == Guid.Parse(id));
            if (трек == null) 
            {
                return new JsonResult(new
                {
                    error = "err"
                });
            }
            bool added = false;
            Треки_в_плейлисте трекВПлейлисте = _context.Треки_в_плейлисте.FirstOrDefault(t => t.ID_Трека == Guid.Parse(id));
            if (трекВПлейлисте != null)
            {
                _context.Remove(трекВПлейлисте);
            }
            else 
            {
                Треки_в_плейлисте трек_в_плейлисте = new Треки_в_плейлисте()
                {
                    ID_Плейлиста = _context.Плейлист.Include(t => t.Пользователь).FirstOrDefault(t => t.ID_Пользователя == UserId).ID,
                    ID_Трека = Guid.Parse(id),
                };
                _context.Add(трек_в_плейлисте);
                added = true;
            }
            _context.SaveChanges();
            return new JsonResult(new
            {
                id = id,
                added = added
            });
        }
        public IActionResult DeleteSong(string? id) 
        {
            if (id == null || id == "") 
            {
                return new JsonResult(new 
                {
                    error = "error"
                });
            }
            Треки трек = _context.Треки.FirstOrDefault(t => t.ID == Guid.Parse(id));
            if (трек == null) 
            {
                return new JsonResult(new
                {
                    error = "error"
                });
            }
            if (трек.удалён)
                трек.удалён = false;
            else
                трек.удалён = true;
            _context.Update(трек);
            _context.SaveChanges();
            return new JsonResult(new 
            {
                id = id,
                deleted = трек.удалён,
            });
        }
    }
}
