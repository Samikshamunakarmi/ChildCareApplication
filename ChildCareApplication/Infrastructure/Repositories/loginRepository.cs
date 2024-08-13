using ChildCareApplication.Application.Interfaces;
using ChildCareApplication.Domain;
using ChildCareApplication.Helper;
using MongoDB.Driver;
using System.Security.Cryptography;

namespace ChildCareApplication.Infrastructure.Repositories
{
    public class loginRepository : Ilogin
    {
        private readonly IMongoDatabase _database;

        public loginRepository(IMongoDatabase database)
        {
            _database = database;
        }
        public async Task<CreateAccount> login(string email)
        {
            var collection = _database.GetCollection<CreateAccount>("CreateAccount");
            var user = collection.AsQueryable().Where(r => r.Email == email).FirstOrDefault();
            if (user == null)
            {
                return null;
            }

            return user;
        }

        public async Task updatePassword(string email, string password, string salt)
        {
            var collection = _database.GetCollection<CreateAccount>("CreateAccount");
            var user = collection.AsQueryable().Where(r => r.Email == email).FirstOrDefault();

            if(user == null)
    {
                throw new Exception("User not found");
            }

            var filter = Builders<CreateAccount>.Filter.Eq(r => r.Email, email);
            var update = Builders<CreateAccount>.Update.Set(r => r.Password, password).Set(r => r.Salt, salt);
          

            var result = await collection.UpdateOneAsync(filter, update);
            if(result.MatchedCount ==0)
            {
                throw new Exception("Update failed");
            }
                
        }
    }
}
