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
                 return PasswordHelper.VerifyPassword(request.Password, user.Password);
            }

            return false;
        }

       

    }

}



