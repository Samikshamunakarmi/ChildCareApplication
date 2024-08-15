using ChildCareApplication.Application.Interfaces;
using ChildCareApplication.Domain;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace ChildCareApplication.Infrastructure.Repositories
{
    public class ParentDetailRepository : IParent
    {
        private readonly IMongoCollection<ParentDetail> _collection;
        public ParentDetailRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<ParentDetail>("Parent");

        }
        public async Task CreateParentDetail(ParentDetail parentInformation)
        {

            await _collection.InsertOneAsync(parentInformation);
        }

        public async Task DeleteParentDetail(string parentId)
        {

            var filter = Builders<ParentDetail>.Filter.Eq(c => c.Id, parentId);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<ParentDetail>> GetAllParentDetails()
        {
            var result = await _collection.Find(_ => true).ToListAsync();
            return result;


        }

        public async Task<ParentDetail> GetParentDetailByIdAsync(string parentId)
        {
            var queryableCollection = _collection.AsQueryable();
            var query = await queryableCollection.Where(x => x.Id == parentId).FirstOrDefaultAsync();
            return query;
        }

        public async Task UpdateParentDetail(ParentDetail parenttInformation)
        {
            var filter = Builders<ParentDetail>.Filter.Eq(c => c.Id, parenttInformation.Id);
            var update = Builders<ParentDetail>.Update.Set(c => c.FirstName, parenttInformation.FirstName)
               .Set(c => c.LastName, parenttInformation.LastName)
               .Set(c => c.Address, parenttInformation.Address)
               .Set(c => c.DateOfBirth, parenttInformation.DateOfBirth)
               .Set(c => c.Relationship, parenttInformation.Relationship)
               .Set(c => c.Email, parenttInformation.Email)
               .Set(c => c.ContactNumber, parenttInformation.ContactNumber)
               .Set(c=>c.ChildFullName, parenttInformation.ChildFullName)
               .Set(c=>c.ChildId, parenttInformation.ChildId)
               .Set(c => c.DateAdded, DateTime.Now);

            await _collection.UpdateOneAsync(filter, update);

        }

        public async Task CreateParentDetailsBatch(List<ParentDetail> parentDetails, IClientSessionHandle session = null)
        {
            var parentCollection = _collection.Database.GetCollection<ParentDetail>("ParentDetails");

            if (session != null)
            {
                await parentCollection.InsertManyAsync(session, parentDetails);
            }
            else
            {
                await parentCollection.InsertManyAsync(parentDetails);
            }
        }

       
    }
}
