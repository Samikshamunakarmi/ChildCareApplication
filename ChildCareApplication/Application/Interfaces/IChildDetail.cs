using ChildCareApplication.Domain;

namespace ChildCareApplication.Application.Interfaces
{
    public interface IChildDetail
    {
        Task CreateChildDetail(ChildInformation childInformation);

        Task<ChildInformation> ReadChildInformationById(string childId);

        Task UpdateChildDetail(ChildInformation childInformation);

        Task DeleteChildDetail(string childId);

        Task<IEnumerable<ChildInformation>> GetAllChildDetails();

    }
}
