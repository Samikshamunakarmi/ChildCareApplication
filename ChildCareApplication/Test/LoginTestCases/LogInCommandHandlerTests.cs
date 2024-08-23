using Bogus;
using ChildCareApplication.Application.CommandHandlers;
using ChildCareApplication.Application.Interfaces;
using ChildCareApplication.Domain;
using FakeItEasy;
using Xunit;

namespace ChildCareApplication.Test.LoginTestCases
{
    public class LogInCommandHandlerTests
    {

        private readonly LogInCommandHandler _handler;
        private readonly Ilogin _loginRepository;

        public LogInCommandHandlerTests()
        {
            _loginRepository = A.Fake<Ilogin>();
            _handler = new LogInCommandHandler(_loginRepository);
        }

        [Fact]
        public async Task Handle_ReturnsTrue_WhenCredentialsAreValid()
        {
            var fakeModel = new Faker<LoginEntities>()
                .RuleFor(m => m.Email, f => f.Internet.Email())
                .RuleFor(m => m.Password, f => f.Internet.Password()).Generate();

            A.CallTo(() => _loginRepository.login(fakeModel.Email)).Returns(Task.FromResult<CreateAccount>(fakeModel));
        }
    }
}
