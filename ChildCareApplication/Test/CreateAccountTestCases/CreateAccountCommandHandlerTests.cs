using Bogus;
using ChildCareApplication.Application.CommandHandlers;
using ChildCareApplication.Application.Dtos;
using ChildCareApplication.Application.Interfaces;
using ChildCareApplication.Domain;
using ChildCareApplication.Infrastructure.Repositories;
using FakeItEasy;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Reflection.Metadata;
using Xunit;

namespace ChildCareApplication.Test.CreateAccountTest
{
    public class CreateAccountCommandHandlerTests
    {
        private readonly CreateAccountCommandHandler _handler;
        private readonly IAccount _accountRepository;
        private readonly IEmailSender _emailSender;

        public CreateAccountCommandHandlerTests()
        {
            
            _accountRepository = A.Fake<IAccount>();
            _emailSender = A.Fake<IEmailSender>();
            _handler = new CreateAccountCommandHandler(_accountRepository, _emailSender);
        }

        [Fact]
        public async Task Handle_ReturnsTrue_WhenAccountIsCreatedSuccessfully()
        {
            // Arrange
            var fakeModel = new Faker<CreateAccountDto>()
                .RuleFor(m => m.UserName, f => f.Internet.UserName())
                .RuleFor(m => m.Password, f => f.Internet.Password())
                .RuleFor(m => m.ConfirmPassword, (f, m) => m.Password)
                .RuleFor(m => m.Email, f => f.Internet.Email())
                .RuleFor(m => m.PhoneNumber, f => f.Random.Int(10000000, 999999999)).Generate();

            
            A.CallTo(() => _accountRepository.ExistingEmailAsync(fakeModel.Email))
                .Returns(Task.FromResult<CreateAccount>(null));

            A.CallTo(() => _accountRepository.CreateAccountAsync(A<CreateAccount>.Ignored))
                .Returns(Task.CompletedTask);

            A.CallTo(() => _emailSender.SendEmailAsync(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored))
                .Returns(Task.CompletedTask); 

            var result = await _handler.Handle(fakeModel, CancellationToken.None);

           
            Assert.True(result);
        }


        [Fact]
        public async Task Handle_ThrowsInvalidOperationException_WhenEmailAlreadyExists()
        {
            // Arrange
            var fakeModel = new Faker<CreateAccountDto>()
                .RuleFor(m => m.UserName, f => f.Internet.UserName())
                .RuleFor(m => m.Password, f => f.Internet.Password())
                .RuleFor(m => m.ConfirmPassword, (f, m) => m.Password)
                .RuleFor(m => m.Email, f => f.Internet.Email())
                .RuleFor(m => m.PhoneNumber, f => f.Random.Int(10000000, 999999999)).Generate();

            // Set up the fake method call to return a non-null value (indicating the email already exists)
            A.CallTo(() => _accountRepository.ExistingEmailAsync(fakeModel.Email))
                .Returns(Task.FromResult(new CreateAccount())); // Simulate an existing account

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(fakeModel, CancellationToken.None));
        }




    }
}
