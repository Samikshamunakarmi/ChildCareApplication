using ChildCareApplication.Application.Interfaces;
using ChildCareApplication.Domain;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ChildCareApplication.Infrastructure.Repositories
{
    public class ChildDetailRepository : IChildDetail

    {
        private readonly IMongoCollection<ChildInformation> _collection;
        public ChildDetailRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<ChildInformation>("ChildDetail");
            
        }
        public async Task CreateChildDetail(ChildInformation childInformation)
        {
           await _collection.InsertOneAsync(childInformation);
        }

        public async Task DeleteChildDetail(string childId)
        {
           
            var filter = Builders<ChildInformation>.Filter.Eq(c => c.Id, childId ); 
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<List<ChildInformation>> GetAllChildDetails()
        {
            var result = await _collection.Find(_ => true).ToListAsync();
            return result;
        }

        public async Task<ChildInformation> GetChildDetailByIdAsync(string childId)
        {

            var filter = Builders<ChildInformation>.Filter.Eq(x => x.Id, childId);
            var result = await _collection.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        public async Task UpdateChildDetail(ChildInformation childInformation)
        {
            var filter = Builders<ChildInformation>.Filter.Eq(c => c.Id , childInformation.Id);
            var update = Builders<ChildInformation>.Update.Set(c => c.FirstName, childInformation.FirstName)
               .Set(c => c.LastName, childInformation.LastName)
               .Set(c => c.Address, childInformation.Address)
               .Set(c => c.Parents, childInformation.Parents)
               .Set(c => c.Allergies, childInformation.Allergies)
               .Set(c => c.DateOfBirth, childInformation.DateOfBirth)
               .Set(c => c.DateAdded, DateTime.Now);

            await _collection.UpdateOneAsync(filter, update);

        }
    }
}
