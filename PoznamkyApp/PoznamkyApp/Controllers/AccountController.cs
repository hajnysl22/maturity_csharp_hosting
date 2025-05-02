using Microsoft.AspNetCore.Mvc;
using PoznamkyApp.Data;
using PoznamkyApp.Models;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;

namespace PoznamkyApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly MongoDbContext _db;

        public AccountController(MongoDbContext db)
        {
            _db = db;
        }

        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(string username, string password, string consent)
        {
            if (consent != "on")
            {
                ModelState.AddModelError(string.Empty, "Musíte souhlasit.");
                return View();
            }

            if (_db.Users.Find(u => u.Username == username).Any())
            {
                ModelState.AddModelError(string.Empty, "Uživatelské jméno je již obsazené.");
                return View();
            }

            var user = new User
            {
                Username = username,
                PasswordHash = HashPassword(password),
                ConsentForRegistering = true
            };

            _db.Users.InsertOne(user);
            return RedirectToAction("Login");
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _db.Users.Find(u => u.Username == username).FirstOrDefault();

            if (user == null || user.PasswordHash != HashPassword(password))
            {
                ModelState.AddModelError(string.Empty, "Nesprávné přihlašovací údaje.");
                return View();
            }

            HttpContext.Session.SetString("UserId", user.Id);
            return RedirectToAction("Index", "Notes");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
