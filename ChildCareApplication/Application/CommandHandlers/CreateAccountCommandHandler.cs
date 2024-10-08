﻿using ChildCareApplication.Application.Dtos;
using ChildCareApplication.Application.Interfaces;
using ChildCareApplication.Domain;
using ChildCareApplication.Helper;
using MediatR;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity.UI.Services;
using MongoDB.Bson;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ChildCareApplication.Application.CommandHandlers
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountDto, bool>
    {

        private readonly IAccount _accountRepository;
        private readonly IEmailSender _emailSender;
        public CreateAccountCommandHandler(IAccount accountRepository, IEmailSender emailSender)
        {
            _accountRepository = accountRepository;
            _emailSender = emailSender;
        }
        public async Task<bool> Handle(CreateAccountDto request, CancellationToken cancellationToken)
        {
           

            if (await _accountRepository.ExistingEmailAsync(request.Email) != null)
            {
                throw new InvalidOperationException("Email already exists.");
            }

            var saltBytes = PasswordHelper.GenerateSalt();
            string hashedPassword = PasswordHelper.HashPassword(request.Password, saltBytes);


            var newAccount = new CreateAccount
            {
                Id =  ObjectId.GenerateNewId().ToString(),
                DateAdded = DateTime.Now,
                UserName = request.UserName,
                Password = hashedPassword,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Salt = Convert.ToBase64String(saltBytes)
            };

            await _accountRepository.CreateAccountAsync(newAccount);
            await _emailSender.SendEmailAsync(request.Email, "Account Created", "Your account has been created successfully.");
            return true ;

        }

      

       
    }
}
