using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoznamkyApp.Data;
using PoznamkyApp.Models;
using System.Linq;

namespace PoznamkyApp.Controllers
{
    public class NotesController : Controller
    {
        private readonly AppDbContext _context;

        public NotesController(AppDbContext context)
        {
            _context = context;
        }

        private int? GetUserId()
        {
            return HttpContext.Session.GetInt32("UserId");
        }

        public IActionResult Index(bool onlyImportant = false)
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var notes = _context.Notes.Where(n => n.UserId == userId);

            if (onlyImportant)
                notes = notes.Where(n => n.IsImportant);

            return View(notes.OrderByDescending(n => n.CreatedAt).ToList());
        }

        [HttpPost]
        public IActionResult Create(string title, string content)
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var note = new Note
            {
                Title = title,
                Content = content,
                UserId = userId.Value
            };

            _context.Notes.Add(note);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var note = _context.Notes.FirstOrDefault(n => n.Id == id && n.UserId == userId);
            if (note != null)
            {
                _context.Notes.Remove(note);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ToggleImportant(int id)
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var note = _context.Notes.FirstOrDefault(n => n.Id == id && n.UserId == userId);
            if (note != null)
            {
                note.IsImportant = !note.IsImportant;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteAccount(string password)
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var user = _context.Users.Include(u => u.Notes).FirstOrDefault(u => u.Id == userId);
            if (user == null || user.PasswordHash != AccountControllerStatic.HashPassword(password))
            {
                ModelState.AddModelError(string.Empty, "Chybné heslo.");
                return RedirectToAction("Index");
            }

            _context.Notes.RemoveRange(user.Notes);
            _context.Users.Remove(user);
            _context.SaveChanges();

            HttpContext.Session.Clear();

            return RedirectToAction("Register", "Account");
        }
    }

    // Malý pomocník kvůli heslu:
    public static class AccountControllerStatic
    {
        public static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
