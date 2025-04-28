using System.ComponentModel.DataAnnotations;

namespace PoznamkyApp.Models
{
    public class Note
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsImportant { get; set; } = false;

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
