using BevCapital.Logon.Domain.Entities;
using Xunit;

namespace BevCapital.Logon.Domain.Tests.Entities
{
    public class AppUserTests
    {
        [Fact(DisplayName = "It should create a user successfully")]
        public void AppUser_ShouldCreate_A_User()
        {
            // ARRANGE
            var name = "name";
            var email = "email";

            // ACT
            var appUser = AppUser.Create(name, email);

            // ASSERT
            Assert.Equal(name, appUser.Name);
            Assert.Equal(email, appUser.Email);
        }

        [Theory(DisplayName = "It should throw an error if user was not created")]
        [InlineData(null, "")]
        [InlineData("", null)]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void AppUser_ShouldNotCreate_A_User_With_EmptyValues(string name, string email)
        {
            // ARRANGE
            // ACT
            var appUser = AppUser.Create(name, email);

            // ASSERT
            Assert.True(appUser.Invalid);
        }
    }
}
