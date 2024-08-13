using ChildCareApplication.Application.Interfaces;
using ChildCareApplication.Domain;
using ChildCareApplication.Helper;
using ChildCareApplication.Infrastructure.Repositories;
using MediatR;

namespace ChildCareApplication.Application.CommandHandlers
{
    public class LogInCommandHandler : IRequestHandler<LoginEntities, bool>
    {
        private readonly Ilogin _loginRepository;

        public LogInCommandHandler(Ilogin loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public async Task<bool> Handle(LoginEntities request, CancellationToken cancellationToken)
        {
            var user = await _loginRepository.login(request.Email);
            if (user != null)
            {
                 return ValidatePassword(user, request.Password);
            }

            return false;
        }

        public static bool ValidatePassword(CreateAccount user, string password)
        {
            if (user == null)
            {
                return false;
            }

            return PasswordHelper.VerifyPassword(password, user.Password);
        }

    }

}



