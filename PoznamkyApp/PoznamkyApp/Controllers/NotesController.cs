using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using PoznamkyApp.Data;
using PoznamkyApp.Models;

namespace PoznamkyApp.Controllers
{
    public class NotesController : Controller
    {
        private readonly MongoDbContext _db;

        public NotesController(MongoDbContext db)
        {
            _db = db;
        }

        private string? GetUserId()
        {
            return HttpContext.Session.GetString("UserId");
        }

        public IActionResult Index(bool onlyImportant = false)
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var filter = Builders<Note>.Filter.Eq(n => n.UserId, userId);
            if (onlyImportant)
                filter = Builders<Note>.Filter.And(filter, Builders<Note>.Filter.Eq(n => n.IsImportant, true));

            var notes = _db.Notes.Find(filter)
                                 .SortByDescending(n => n.CreatedAt)
                                 .ToList();

            return View(notes);
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
                UserId = userId,
                CreatedAt = DateTime.Now
            };

            _db.Notes.InsertOne(note);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var filter = Builders<Note>.Filter.And(
                Builders<Note>.Filter.Eq(n => n.Id, id),
                Builders<Note>.Filter.Eq(n => n.UserId, userId)
            );

            _db.Notes.DeleteOne(filter);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ToggleImportant(string id)
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var filter = Builders<Note>.Filter.And(
                Builders<Note>.Filter.Eq(n => n.Id, id),
                Builders<Note>.Filter.Eq(n => n.UserId, userId)
            );

            var note = _db.Notes.Find(filter).FirstOrDefault();
            if (note != null)
            {
                note.IsImportant = !note.IsImportant;
                _db.Notes.ReplaceOne(filter, note);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteAccount(string password)
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var user = _db.Users.Find(u => u.Id == userId).FirstOrDefault();

            if (user == null || user.PasswordHash != AccountControllerStatic.HashPassword(password))
            {
                ModelState.AddModelError(string.Empty, "Chybné heslo.");
                return RedirectToAction("Index");
            }

            _db.Notes.DeleteMany(n => n.UserId == userId);
            _db.Users.DeleteOne(u => u.Id == userId);

            HttpContext.Session.Clear();
            return RedirectToAction("Register", "Account");
        }
    }

    // Helper to hash passwords (static version for cross-class use)
    public static class AccountControllerStatic
    {
        public static string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
