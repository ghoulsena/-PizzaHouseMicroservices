using MediatR;
using Moq;
using System.Runtime.Intrinsics.X86;
using UserService.Application.Interfaces;
using UserService.Application.Services;
using UserService.Domain.Entities;
using UserService.Domain.ValueObjects;

namespace UserServiceUnitTest.ServicesTest
{
    public class UserAppServiceTest
    {
        [Fact]
        public async Task Login_ChechNullUser_ReturnException()
        {
            var mockRepo = new Mock<IUserRepository>();
            var mockAuth = new Mock<IAuthProvider>();
            var mockMediator = new Mock<IMediator>();

            mockRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);

            var service = new UserAppService(
              mockRepo.Object,
              mockAuth.Object,
              mockMediator.Object
          );

            await Assert.ThrowsAsync<Exception>(() =>
                service.Login("test@example.com", "password"));


        }

        [Fact]
        public async Task Login_CheckNullInvalidPassword_ReturnException()
        {
            var mockRepo = new Mock<IUserRepository>();
            var mockAuth = new Mock<IAuthProvider>();
            var mockMediator = new Mock<IMediator>();



            var service = new UserAppService(
              mockRepo.Object,
              mockAuth.Object,
              mockMediator.Object
          );

            var user = CreateUser();

            mockRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);


            await Assert.ThrowsAsync<Exception>(() =>
                service.Login("test@example.com", "InvalidPassword"));
        }
        [Fact]
        public async Task Login_WithValidUser_ReturnsToken()
        {
            var user = CreateUser();

            var mockRepo = new Mock<IUserRepository>();
            var mockMediator = new Mock<IMediator>();
            var mockAuth = new Mock<IAuthProvider>();
            mockAuth.Setup(r => r.GenerateToken(user)).Returns("token");



            var service = new UserAppService(
              mockRepo.Object,
              mockAuth.Object,
              mockMediator.Object
          );



            mockRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

            var token = await service.Login(user.EmailValue, user.PasswordValue);
            Assert.Equal("token", token);
        }

        [Fact]
        public async Task RegisterAsync_EmailAvailabilityCheck_ReturnException()
        {
            var mockRepo = new Mock<IUserRepository>();
            var mockAuth = new Mock<IAuthProvider>();
            var mockMediator = new Mock<IMediator>();



            var service = new UserAppService(
              mockRepo.Object,
              mockAuth.Object,
              mockMediator.Object
          );

            var user = CreateUser();

            mockRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

            await Assert.ThrowsAsync<Exception>(() => service.RegisterAsync(user.UserName, user.EmailValue, user.PasswordValue));

        }
        [Fact]
        public async Task RegisterAsync_EmailAvailabilityCheck_ReturnUser()
        {
            var mockRepo = new Mock<IUserRepository>();
            var mockAuth = new Mock<IAuthProvider>();
            var mockMediator = new Mock<IMediator>();



            var service = new UserAppService(
              mockRepo.Object,
              mockAuth.Object,
              mockMediator.Object
          );

            var user = CreateUser();


            mockRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);
            

            var result = await service.RegisterAsync(user.UserName, user.EmailValue, user.PasswordValue);

            mockMediator.Verify(m => m.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
            mockRepo.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
            Assert.Equal(user.UserName, result.UserName);
            Assert.Equal(user.EmailValue, result.Email.Value);
        }
        [Fact]
        public async Task AddRoleToUserAsync_CheckNullUser_ReturnException()
        {
            var mockRepo = new Mock<IUserRepository>();
            var mockAuth = new Mock<IAuthProvider>();
            var mockMediator = new Mock<IMediator>();

            var user = CreateUser();

            var service = new UserAppService(
              mockRepo.Object,
              mockAuth.Object,
              mockMediator.Object
          );

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User)null);
            var result = await service.AddRoleToUserAsync(user.Id, "Role");

            Assert.Equal(false, result.Success);
        }

        [Fact]
        public async Task AddRoleToUserAsync_CheckOnTrue_TrueAndMessage()
        {
            var mockRepo = new Mock<IUserRepository>();
            var mockAuth = new Mock<IAuthProvider>();
            var mockMediator = new Mock<IMediator>();

            var user = CreateUser();

            var service = new UserAppService(
              mockRepo.Object,
              mockAuth.Object,
              mockMediator.Object
          );

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            var result = await service.AddRoleToUserAsync(user.Id, "Role");

            Assert.Equal(true, result.Success);

        }

        public User CreateUser()
        {
            var email = new Email("test@example.com");
            var password = new Password("CorrectPassword");
            var user = User.Create("testuser", email, password);
            return user;
        }
    }
}
