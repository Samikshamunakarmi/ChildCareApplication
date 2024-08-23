using ChildCareApplication.Presentation.Controllers;
using FakeItEasy;
using Xunit;
using MediatR;
using Bogus;
using ChildCareApplication.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ChildCareApplication.Test.CreateAccountTestCases
{
    public class AccountControllerTests
    {

        private readonly AccountController _accountController;
        private readonly IMediator _mediator;

        public AccountControllerTests()
        {
            _mediator = A.Fake<IMediator>();
            _accountController = new AccountController(_mediator);
        }

        [Fact]
        public async Task CreateAccount_ReturnsOkResult_WhenAccountIsCreatedSuccessfully()
        {
            // Arrange
            var fakeModel = new Faker<CreateAccountDto>()
                .RuleFor(m => m.UserName, f => f.Internet.UserName())
                .RuleFor(m => m.Password, f => f.Internet.Password())
                .RuleFor(m => m.ConfirmPassword, (f, m) => m.Password) // Ensure password matches
                .RuleFor(m => m.Email, f => f.Internet.Email())
                .RuleFor(m => m.PhoneNumber, f => f.Random.Int(100000000, 999999999))
                .Generate();


            A.CallTo(() => _mediator.Send(A<CreateAccountDto>.Ignored, default)).Returns(Task.FromResult(true));

            //Act

            var result = await _accountController.CreateAccount(fakeModel);

            //Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, actionResult.StatusCode);


        }

        [Fact]
        public async Task CreateAccount_ReturnsBadRequest_WhenPasswordsDoNotMatch()
        {
            //Arrange
            var fakeModel = new Faker<CreateAccountDto>()
                .RuleFor(m => m.UserName, f => f.Internet.UserName())
                .RuleFor(m => m.Password, f => f.Internet.Password())
                .RuleFor(M => M.ConfirmPassword, F => F.Internet.Password())
                .RuleFor(m => m.Email, f => f.Internet.Email())
                .RuleFor(m => m.PhoneNumber, f => f.Random.Int(100000000, 999999999)).Generate();

            A.CallTo(() => _mediator.Send(A<CreateAccountDto>.Ignored, default)).Throws(new
                InvalidOperationException("Password do not match"));

            //Act
            var result = await _accountController.CreateAccount(fakeModel);

            //Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, actionResult.StatusCode);
            Assert.Contains("Password do not match", actionResult.Value.ToString());

        }

        [Fact]

        public async Task CreateAccount_ReturnsBadRequest_WhenEmailIsInvalid()
        {

            //Arrange
            //Arrange
            var fakeModel = new Faker<CreateAccountDto>()
                .RuleFor(m => m.UserName, f => f.Internet.UserName())
                .RuleFor(m => m.Password, f => f.Internet.Password())
                .RuleFor(M => M.ConfirmPassword, F => F.Internet.Password())
                .RuleFor(m => m.Email, f => "Invalid-email")
                .RuleFor(m => m.PhoneNumber, f => f.Random.Int(100000000, 999999999)).Generate();


            A.CallTo(() => _mediator.Send(A<CreateAccountDto>.Ignored, default)).Throws(new InvalidOperationException("email is incorrect format"));

            //Act

            var result = await _accountController.CreateAccount(fakeModel);

            //Assert

            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, actionResult.StatusCode);

            Assert.Contains("email is incorrect format", actionResult.Value.ToString());

        }
    }
}
