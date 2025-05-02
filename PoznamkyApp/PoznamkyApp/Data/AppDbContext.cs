using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using PoznamkyApp.Models;

namespace PoznamkyApp.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDb"));
            _database = client.GetDatabase("PoznamkyAppDb");
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Note> Notes => _database.GetCollection<Note>("Notes");
    }
}
