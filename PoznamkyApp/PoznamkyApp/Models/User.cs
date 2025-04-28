using System.ComponentModel.DataAnnotations;

namespace PoznamkyApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public bool ConsentForRegistering { get; set; }

        public List<Note> Notes { get; set; }
    }
}
