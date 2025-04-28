using Microsoft.AspNetCore.Mvc;
using PoznamkyApp.Data;
using PoznamkyApp.Models;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace PoznamkyApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string username, string password, string consent)
        {
            if (consent != "on")
            {
                ModelState.AddModelError(string.Empty, "Musíte souhlasit.");
                return View();
            }

            if (_context.Users.Any(u => u.Username == username))
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

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username);

            if (user == null || user.PasswordHash != HashPassword(password))
            {
                ModelState.AddModelError(string.Empty, "Nesprávné přihlašovací údaje.");
                return View();
            }

            HttpContext.Session.SetInt32("UserId", user.Id);

            return RedirectToAction("Index", "Notes");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
