using System;
using System.Collections.Generic;
using System.Text;
using UserService.Domain.Entities;
using UserService.Domain.Event;
using UserService.Domain.ValueObjects;

namespace UserServiceUnitTest.EntitiesTest
{
    public class UserTest
    {
        [Fact]
        public void Create_ShouldCreateUser_AndRaiseDomainEvent()
        {

            var user = CreateUser();
            Assert.NotEqual(Guid.Empty, user.Id);
            Assert.Equal("testuser", user.UserName);
            Assert.Single(user.DomainEvents); //проверяем что событие одно
            Assert.IsType<UserRegisteredDomainEvent>(user.DomainEvents.First()); // проверяем что оно правильного типа
        }
        [Fact]
        public void AddRole_ShouldAddRole_WhenRoleDoesNotExist()
        {
            var user = CreateUser();
            var role = new Role("admin");

            user.AddRole(role);

            Assert.Contains(user.Roles, r => r.Name == "admin");
        }


        [Fact]
        public void MethodName_WhenCondition_ShouldExpectedBehavior()
        {
            var user = CreateUser();
            var role = new Role("admin");

            user.AddRole(role);
            user.AddRole(role); 

            Assert.Single(user.Roles); 
        }

        [Fact]
        public void CheckPassword_WhenPasswordDoesNotMatch_ShouldReturnFalse()
        {
            var user = CreateUser();
            var result = user.CheckPassword("FalsePassword");
            Assert.False(result);
        }

        [Fact]
        public void CheckPassword_WhenPasswordMatches_ShouldReturnTrue()
        {
            var user = CreateUser();
            var result = user.CheckPassword("CorrectPassword");
            Assert.True(result);
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
