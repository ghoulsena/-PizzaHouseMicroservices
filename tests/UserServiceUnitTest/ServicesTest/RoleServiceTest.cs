using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using UserService.Application.Interfaces;
using UserService.Application.Services;
using UserService.Domain.Entities;
using UserService.Domain.ValueObjects;

namespace UserServiceUnitTest.ServicesTest
{
    public class RoleServiceTest
    {
        [Fact]
        public async Task AssignRoleToUserAsync_WhenUserIsNull_ShouldReturnFailure()
        {
            var mockRoleRepo = new Mock<IRoleRepository>();
            var mockUserRepo = new Mock<IUserRepository>();
            var service = new RoleService(mockRoleRepo.Object, mockUserRepo.Object);
            mockUserRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User)null);


            var result = await service.AssignRoleToUserAsync(It.IsAny<Guid>(), "role");
            
            Assert.False(result.Success);
            Assert.Equal("Пользователь не найден", result.Message);
            mockRoleRepo.Verify(r => r.GetByNameAsync(It.IsAny<string>()), Times.Never);
            mockRoleRepo.Verify(r => r.AddAsync(It.IsAny<Role>()), Times.Never);
            mockUserRepo.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);

        }

        [Fact]
        public async Task AssignRoleToUserAsync_WhenRoleDoesNotExist_ShouldAddRoleAndReturnSuccess()
        {
            var mockRoleRepo = new Mock<IRoleRepository>();
            var mockUserRepo = new Mock<IUserRepository>();
            var user = CreateUser();
            var roleName = "role";
            var service = new RoleService(mockRoleRepo.Object, mockUserRepo.Object);    
            mockUserRepo.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

            mockRoleRepo.Setup(r => r.GetByNameAsync(roleName)).ReturnsAsync((Role)null);
            mockRoleRepo.Setup(r => r.AddAsync(It.IsAny<Role>())).Returns(Task.CompletedTask);
            var result = await service.AssignRoleToUserAsync(user.Id, "role");
            mockUserRepo.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask);

            Assert.True(result.Success);
            Assert.Equal($"Пользователю {user.UserName} присвоена роль {roleName}", result.Message);
            mockRoleRepo.Verify(r => r.AddAsync(It.Is<Role>(role => role.Name == roleName)), Times.Once);
            mockUserRepo.Verify(r => r.UpdateAsync(user), Times.Once);

        }

        [Fact]
        public async Task AssignRoleToUserAsync_WhenRoleExists_ShouldNotAddRoleButReturnSuccess()
        {
            var mockRoleRepo = new Mock<IRoleRepository>();
            var mockUserRepo = new Mock<IUserRepository>();
            var user = CreateUser();
            var roleName = "existingRole";
            var existingRole = new Role(roleName);  
            var service = new RoleService(mockRoleRepo.Object, mockUserRepo.Object);

            mockUserRepo.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
            mockRoleRepo.Setup(r => r.GetByNameAsync(roleName)).ReturnsAsync(existingRole);
            mockUserRepo.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask);

            var result = await service.AssignRoleToUserAsync(user.Id, roleName);

            Assert.True(result.Success);
            Assert.Equal($"Пользователю {user.UserName} присвоена роль {roleName}", result.Message);

            mockRoleRepo.Verify(r => r.AddAsync(It.IsAny<Role>()), Times.Never);
            mockUserRepo.Verify(r => r.UpdateAsync(user), Times.Once);
        }

        [Fact]
        public async Task AssignRoleToUserAsync_WhenUserAlreadyHasRole_ShouldNotDuplicateRole()
        {
            var mockRoleRepo = new Mock<IRoleRepository>();
            var mockUserRepo = new Mock<IUserRepository>();
            var user = CreateUser();
            var roleName = "role";
            var role = new Role(roleName);
            user.AddRole(role); 
            var service = new RoleService(mockRoleRepo.Object, mockUserRepo.Object);

            mockUserRepo.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
            mockRoleRepo.Setup(r => r.GetByNameAsync(roleName)).ReturnsAsync(role);
            mockUserRepo.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask);

            var result = await service.AssignRoleToUserAsync(user.Id, roleName);

            Assert.True(result.Success);
            Assert.Equal($"Пользователю {user.UserName} присвоена роль {roleName}", result.Message);

            mockRoleRepo.Verify(r => r.AddAsync(It.IsAny<Role>()), Times.Never);
            mockUserRepo.Verify(r => r.UpdateAsync(user), Times.Once);
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
