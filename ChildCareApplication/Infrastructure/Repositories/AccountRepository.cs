using ChildCareApplication.Application.Interfaces;
using ChildCareApplication.Domain;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace ChildCareApplication.Infrastructure.Repositories
{
    public class AccountRepository : IAccount
    {
        private readonly IMongoDatabase _database;
        private readonly ILogger<CreateAccount> _logger;
        public AccountRepository(IMongoDatabase database,ILogger<CreateAccount> logger)
        {
            _database = database; 
            _logger = logger;
        }
    
        public async Task CreateAccountAsync(CreateAccount account)
        {
            var collection = _database.GetCollection<CreateAccount>("CreateAccount");
            await collection.InsertOneAsync(account);
        }

        public async Task<CreateAccount> ExistingEmailAsync(string email)
        {
            var collection = _database.GetCollection<CreateAccount>("CreateAccount");

            // Use LINQ to query MongoDB
            var searchExistingEmailIsPresent = await collection.AsQueryable()
                .Where(x => x.Email == email)
                .FirstOrDefaultAsync();

            if (searchExistingEmailIsPresent != null)
            {
                _logger.LogInformation("Email found: {Email}", email);
            }
            else
            {
                _logger.LogInformation("Email not found: {Email}", email);
            }

            return searchExistingEmailIsPresent;
        }
    }
}
