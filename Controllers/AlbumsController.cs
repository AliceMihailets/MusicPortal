using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicPortal.Models;

namespace MusicPortal.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly musicalportalcontext _context;

        public AlbumsController(musicalportalcontext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Альбомы> musicalportalcontext = _context.Альбомы.Include(а => а.жанр)
                .Include(а => а.коллектив).Include(а => а.тип_альбома).Where(t => t.удален == false).ToList();
            List<AlbumsWithTreksViewModel> albums = new List<AlbumsWithTreksViewModel>();
            foreach (var album in musicalportalcontext) 
            {
                var tmpsrc = ASCIIEncoding.ASCII.GetBytes(album.Название_альбома);
                var tmphash = new MD5CryptoServiceProvider().ComputeHash(tmpsrc);
                Int32 int_hash = BitConverter.ToInt32(tmphash) % 10;
                var album_hernya = _context.Треки.Where(t => t.ID_альбома == album.ID).Where(t => t.удалён == false).Select(t => new { t.ID, t.ID_альбома, t.Название_трека }).ToList();
                List<Треки> треки = new List<Треки>();
                foreach (var item in album_hernya) 
                {
                    треки.Add(new Треки()
                    {
                        ID = item.ID,
                        ID_альбома = item.ID_альбома,
                        Название_трека = item.Название_трека
                    });
                }
                albums.Add(new AlbumsWithTreksViewModel()
                {
                    альбом = album,
                    треки = треки,
                    pic_num = Math.Abs(int_hash) + 1
                });
            }
            List<string> types = _context.Типы_альбомов.Select(t => t.Тип_альбома).ToList();
            List<string> genres = _context.Жанры.Select(t => t.Жанр).ToList();
            ViewBag.Types = types;
            ViewBag.Genres = genres;
            return View(albums);
        }
        public IActionResult FilterAlbum(string? pattern, string? genre, string? type) 
        {
            IQueryable<Альбомы> albumsQuer = _context.Альбомы.Include(t => t.жанр)
                                                        .Include(t => t.коллектив)
                                                        .Include(t => t.тип_альбома)
                                                        .Where(t=>t.удален == false);
            if (pattern != null) 
            {
                albumsQuer = albumsQuer.Where(t => EF.Functions.Like(t.Название_альбома, pattern + "%"));
            }
            if (genre != null) 
            {
                albumsQuer = albumsQuer.Where(t => t.жанр.Жанр == genre);
            }
            if (type != null)
            {
                albumsQuer = albumsQuer.Where(t => t.тип_альбома.Тип_альбома == type); 
            }
            List<Альбомы> albums = albumsQuer.ToList();
            List<AlbumsWithTreksViewModel> albumswithtraks = new List<AlbumsWithTreksViewModel>();
            foreach (var album in albums)
            {
                var tmpsrc = ASCIIEncoding.ASCII.GetBytes(album.Название_альбома);
                var tmphash = new MD5CryptoServiceProvider().ComputeHash(tmpsrc);
                Int32 int_hash = BitConverter.ToInt32(tmphash) % 10;
                var album_hernya = _context.Треки.Where(t => t.ID_альбома == album.ID).Where(t => t.удалён == false).Select(t => new { t.ID, t.ID_альбома, t.Название_трека }).ToList();
                List<Треки> треки = new List<Треки>();
                foreach (var item in album_hernya)
                {
                    треки.Add(new Треки()
                    {
                        ID = item.ID,
                        ID_альбома = item.ID_альбома,
                        Название_трека = item.Название_трека
                    });
                }
                albumswithtraks.Add(new AlbumsWithTreksViewModel()
                {
                    альбом = album,
                    треки = треки,
                    pic_num = Math.Abs(int_hash) + 1
                });
            }
            return PartialView("_AlbumsFiltered",albumswithtraks);
        }
        public IActionResult GetGenresAndTypes() 
        {
            List<string> жанры = _context.Жанры.Select(t => t.Жанр).ToList();
            List<string> типы = _context.Типы_альбомов.Select(t => t.Тип_альбома).ToList();
            List<string> группы = _context.Коллективы.Select(t => t.Название_коллектива).ToList();
            return new JsonResult(new
            {
                genre = жанры,
                type = типы,
                band = группы,
            });
        }
        public IActionResult EditFields(string? id,string? name, string? genre, string? type,string? band) 
        {
            Альбомы альбом = _context.Альбомы.FirstOrDefault(t => t.ID == Guid.Parse(id));
            if (name != "" && name != null) 
            {
                альбом.Название_альбома = name;
            }
            if (genre != null && genre != "") 
            {
                альбом.ID_Жанра = _context.Жанры.FirstOrDefault(t => t.Жанр == genre).ID_жанра;
            }
            if (type != null && type != "") 
            {
                альбом.ID_Типа_альбома = _context.Типы_альбомов.FirstOrDefault(t => t.Тип_альбома == type).ID_типа_альбомы;
            }
            if (band != null && band != "") 
            {
                альбом.ID_коллектива = _context.Коллективы.FirstOrDefault(t => t.Название_коллектива == band).ID;
            }
            _context.Update(альбом);
            _context.SaveChanges();
            return new JsonResult(new
            {
                name = альбом.Название_альбома,
                genre = _context.Альбомы.Include(t=>t.жанр).FirstOrDefault(t=>t.ID == альбом.ID).жанр.Жанр,
                type = _context.Альбомы.Include(t => t.тип_альбома).FirstOrDefault(t => t.ID == альбом.ID).тип_альбома.Тип_альбома,
                band = _context.Альбомы.Include(t=>t.коллектив).FirstOrDefault(t=>t.ID == альбом.ID).коллектив.Название_коллектива
            });
        }
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var альбомы = _context.Альбомы
                .Include(а => а.жанр)
                .Include(а => а.коллектив)
                .Include(а => а.тип_альбома)
                .FirstOrDefault(m => m.ID == id);
            if (альбомы == null)
            {
                return NotFound();
            }
            AlbumsWithTreksViewModel album;
            var tmpsrc = ASCIIEncoding.ASCII.GetBytes(альбомы.Название_альбома);
            var tmphash = new MD5CryptoServiceProvider().ComputeHash(tmpsrc);
            Int32 int_hash = BitConverter.ToInt32(tmphash) % 10;
            var album_hernya = _context.Треки.Where(t => t.ID_альбома == альбомы.ID).Where(t => t.удалён == false).Select(t => new { t.ID, t.ID_альбома, t.Название_трека }).ToList();
            List<Треки> треки = new List<Треки>();
            foreach (var item in album_hernya)
            {
                треки.Add(new Треки()
                {
                    ID = item.ID,
                    ID_альбома = item.ID_альбома,
                    Название_трека = item.Название_трека
                });
            }
            album = new AlbumsWithTreksViewModel()
            {
                альбом = альбомы,
                треки = треки,
                pic_num = Math.Abs(int_hash) + 1
            };
            return View(album);
        }
        public IActionResult Create()
        {
            ViewData["ID_Жанра"] = new SelectList(_context.Жанры, "ID_жанра", "Жанр");
            ViewData["ID_коллектива"] = new SelectList(_context.Коллективы, "ID", "Название_коллектива");
            ViewData["ID_Типа_альбома"] = new SelectList(_context.Типы_альбомов, "ID_типа_альбомы", "Тип_альбома");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Название_альбома,ID_Жанра,ID_Типа_альбома,ID_коллектива,Дата_выпуска")] Альбомы альбомы)
        {
            if (ModelState.IsValid)
            {
                альбомы.ID = Guid.NewGuid();
                _context.Add(альбомы);
                await _context.SaveChangesAsync();
                return RedirectToAction("Adding", "Home");
            }
            ViewData["ID_Жанра"] = new SelectList(_context.Жанры, "ID_жанра", "ID_жанра", альбомы.ID_Жанра);
            ViewData["ID_коллектива"] = new SelectList(_context.Коллективы, "ID", "ID", альбомы.ID_коллектива);
            ViewData["ID_Типа_альбома"] = new SelectList(_context.Типы_альбомов, "ID_типа_альбомы", "ID_типа_альбомы", альбомы.ID_Типа_альбома);
            return View(альбомы);
        }
        [Authorize(Roles = "admin")]
        public IActionResult DeleteAlbum(string? id) 
        {
            Альбомы альбом = _context.Альбомы.FirstOrDefault(t => t.ID == Guid.Parse(id));
            альбом.удален = true;
            _context.SaveChanges();
            return new JsonResult(new
            {
            });
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var альбомы = await _context.Альбомы
                .Include(а => а.жанр)
                .Include(а => а.коллектив)
                .Include(а => а.тип_альбома)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (альбомы == null)
            {
                return NotFound();
            }

            return View(альбомы);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var альбомы = await _context.Альбомы.FindAsync(id);
            _context.Альбомы.Remove(альбомы);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool АльбомыExists(Guid id)
        {
            return _context.Альбомы.Any(e => e.ID == id);
        }
    }
}
