using Bogus;
using ChildCareApplication.Domain;
using FakeItEasy;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Xunit;

namespace ChildCareApplication.Test.LoginTestCases
{
    public class LogInControllerTests
    {
        private readonly LoginController _loginController;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public LogInControllerTests()
        {
            
            _configuration = A.Fake<IConfiguration>();
            _mediator = A.Fake<IMediator>();
            _loginController = new LoginController(_configuration, _mediator);
        }

        [Fact]

        public async Task LoginIn_ReturnOkResult_WhenLogInSuccessfully()
        {
            var fakeModel = new Faker<LoginEntities>()
                .RuleFor(m => m.Email, f => f.Internet.Email())
                .RuleFor(m => m.Password, f => f.Internet.Password()).Generate();

            A.CallTo(() => _mediator.Send(A<LoginEntities>.Ignored,default)).Returns(Task.FromResult(true));

            //Act
            var result = await _loginController.Login(fakeModel);

            //Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, actionResult.StatusCode);



        }

        public async Task Login_ThrowsException_ReturnsUnauthorized()
        {
            var fakeModel = new Faker<LoginEntities>()
                .RuleFor(m => m.Email, f => f.Internet.Email())
                .RuleFor(m => m.Password, f => f.Internet.Password()).Generate();

            A.CallTo(() => _mediator.Send(A<LoginEntities>.Ignored, default)).Returns(Task.FromResult(false));

            //Act
            var result = await _loginController.Login(fakeModel);

            //Assert
            Assert.IsType<UnauthorizedResult>(result);


        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var login = new LoginEntities { Email = "test@example.com", Password = "invalidPassword" };
            A.CallTo(() => _mediator.Send(login, default)).Returns(false);

            // Act
            var result = await _loginController.Login(login);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

    }
}
