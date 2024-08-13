using ChildCareApplication.Domain;
using System.Globalization;

namespace ChildCareApplication.Application.Interfaces
{
    public interface Ilogin
    {
        Task<CreateAccount> login(string email);

        Task updatePassword(string email, string password, string salt);
    }
}
