using ChildCareApplication.Application.Interfaces;
using ChildCareApplication.Domain;
using ChildCareApplication.Helper;
using ChildCareApplication.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity.UI.Services;
using MongoDB.Bson;

namespace ChildCareApplication.Application.CommandHandlers
{
    public class ResetPasswordCommanHandler : IRequestHandler<ReSetPassword, bool>
    {
        private readonly Ilogin _loginRepository;
        private readonly IEmailSender _emailSender;

        public ResetPasswordCommanHandler(Ilogin loginRepository, IEmailSender emailSender)
        {
            _loginRepository = loginRepository;
            _emailSender = emailSender;
        }
        public async Task<bool> Handle(ReSetPassword request, CancellationToken cancellationToken)
        {
            var user = await _loginRepository.login(request.Email);
            if (user != null)
            {

                var saltBytes = PasswordHelper.GenerateSalt();
                string hashedPassword = PasswordHelper.HashPassword(request.NewPassword, saltBytes);
                

                await _loginRepository.updatePassword(user.Email, hashedPassword, Convert.ToBase64String(saltBytes));
                await _emailSender.SendEmailAsync(request.Email, "Password change", "Your account password was changed successfully");
                return true;
            }

            return false;
        }
    }
}
