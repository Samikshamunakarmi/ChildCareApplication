using ChildCareApplication.Domain;
using MongoDB.Driver;

namespace ChildCareApplication.Application.Interfaces
{
    public interface IParent
    {

        Task CreateParentDetail(ParentDetail parentInformation);

        Task<IEnumerable<ParentDetail>> GetAllParentDetails();


        Task<ParentDetail> GetParentDetailByIdAsync(string ParentId);

        Task UpdateParentDetail(ParentDetail parentInformation);

        Task DeleteParentDetail(string ParentId);
        Task CreateParentDetailsBatch(List<ParentDetail> parentDetails, IClientSessionHandle session = null);
    }
}
