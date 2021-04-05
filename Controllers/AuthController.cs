using System;
using System.Collections.Generic;
using System.Linq;
using MusicPortal.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace MusicPortal.Controllers
{
    public class AuthController : Controller
    {
        private musicalportalcontext _context;
        private readonly byte[] _tokensalt;
        public AuthController(musicalportalcontext context) 
        {
            _tokensalt = Convert.FromBase64String("h/QUm2iYZ8lIttbVGCTnYA==");
            _context = context;
        }
        [HttpGet]
        public IActionResult Register() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(Пользователь пользователь, IFormFile аватарка)
        {
            var salt = GetSalt();
            var hashed_password = KeyDerivation.Pbkdf2(
                password: пользователь.Пароль,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);
            string stringPasswordHashed = Convert.ToBase64String(hashed_password);
            пользователь.Пароль = stringPasswordHashed;
            пользователь._salt = Convert.ToBase64String(salt);
            if (аватарка != null) 
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(аватарка.OpenReadStream())) 
                {
                    imageData = binaryReader.ReadBytes((int)аватарка.Length);
                }
                пользователь.аватарка = imageData;
            }
            _context.Пользователь.Add(пользователь);
            Пользователь другой_пользователь = _context.Пользователь.SingleOrDefault(t => t.Никнейм == пользователь.Никнейм);
            if (!ModelState.IsValid)
            {
                return View("Register");
            }
            if (другой_пользователь == null)
            {
                _context.SaveChanges();
            }
            else
            {
                ViewBag.Reqister_user = пользователь;
                ModelState.AddModelError("", "Есть уже такой никнейм перепишите его пожалуйста будте добры сэр.");
                return View("Register");
            }
            return RedirectToAction("Index", "Home");
        }
        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckNickname(string Никнейм)
        {
            string nick = _context.Пользователь.Single(t => t.Никнейм == Никнейм).Никнейм;
            if (nick != null)
            {
                return Json(false);
            }
            return Json(false);
        }
        public byte[] GetSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
        private async Task Authenticate(string userName, string rolename)
        {
            // создаем один claim
            if (rolename != "admin")
                rolename = "user";
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, rolename)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var hashed = KeyDerivation.Pbkdf2(
                password: HttpContext.Connection.RemoteIpAddress + HttpContext.Request.Headers["User-Agent"],
                salt: _tokensalt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);
            string stringhashed = Convert.ToBase64String(hashed);
            var FoundUser = _context.Пользователь.FirstOrDefault(t => t._token == stringhashed);
            if (FoundUser != null)
            {
                Guid IdUser = FoundUser.ID;
                string UserName = FoundUser.Никнейм;
                await Authenticate(IdUser.ToString(), UserName);
                var gh = HttpContext.User;
                return Redirect("/");
            }
            else
                return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string Никнейм, string Пароль)
        {
            string resultname = Никнейм + Пароль;
            Пользователь пользователь = _context.Пользователь.Single(t => t.Никнейм == Никнейм);
            if (пользователь != null)
            {
                var salt = Convert.FromBase64String(пользователь._salt);
                var passwdhash = KeyDerivation.Pbkdf2(
                    password: Пароль,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8);
                string stringpasswdhashed = Convert.ToBase64String(passwdhash);
                if (пользователь.Пароль != stringpasswdhashed)
                {
                    ViewBag.Error = "Wrong password";
                    return View();
                }
                Guid IdUser = пользователь.ID;
                string UserName = пользователь.Никнейм;
                await Authenticate(IdUser.ToString(), UserName);
                var gh = HttpContext.User.Identity.Name;
                var hashed = KeyDerivation.Pbkdf2(
                password: HttpContext.Connection.RemoteIpAddress + HttpContext.Request.Headers["User-Agent"],
                salt: _tokensalt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);
                string stringhashed = Convert.ToBase64String(hashed);
                пользователь._token = stringhashed;
                _context.Пользователь.Update(пользователь);
                _context.SaveChanges();
            }
            else
            {
                ViewBag.Error = "Wrong login";
                return View();
            }
            return Redirect("/");
        }
        public async Task<IActionResult> Logout()
        {
            var username = Guid.Parse(HttpContext.User.Identity.Name);
            Пользователь пользователь = _context.Пользователь.FirstOrDefault(t => t.ID == username);
            пользователь._token = null;
            _context.Пользователь.Update(пользователь);
            _context.SaveChanges();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
