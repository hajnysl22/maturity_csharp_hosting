using MongoDB.Driver;
using PoznamkyApp.Models;

namespace PoznamkyApp.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Note> Notes => _database.GetCollection<Note>("Notes");

        public MongoDbContext()
        {
            var connectionString = Environment.GetEnvironmentVariable("MONGO_DB");

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("MongoDB connection string not found in environment variables.");

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("PoznamkyAppDb");
        }
    }
}
