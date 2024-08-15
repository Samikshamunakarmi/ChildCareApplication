using ChildCareApplication.Domain;

namespace ChildCareApplication.Application.Interfaces
{
    public interface IChildDetail
    {
        Task CreateChildDetail(ChildInformation childInformation);

        Task<List<ChildInformation>> GetAllChildDetails();


        Task<ChildInformation> GetChildDetailByIdAsync(string childId);

        Task UpdateChildDetail(ChildInformation childInformation);

        Task DeleteChildDetail(string childId);

        
    }
}
