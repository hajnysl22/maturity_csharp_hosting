using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace PoznamkyApp.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public bool ConsentForRegistering { get; set; }
    }
}
