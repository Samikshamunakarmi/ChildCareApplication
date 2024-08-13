using ChildCareApplication.Domain;

namespace ChildCareApplication.Application.Interfaces
{
    public interface IAccount
    {
        Task CreateAccountAsync(CreateAccount account);
        Task<CreateAccount> ExistingEmailAsync(string email);
    }
}
