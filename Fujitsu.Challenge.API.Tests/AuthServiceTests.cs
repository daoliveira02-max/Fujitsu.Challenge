using Fujitsu.Challenge.API.Models;
using Fujitsu.Challenge.API.Models.Auth;
using Fujitsu.Challenge.API.Models.Settings;
using Fujitsu.Challenge.API.Services;
using Microsoft.Extensions.Options;

namespace Fujitsu.Challenge.API.Tests
{
    [TestClass]
    public class AuthServiceTests
    {
        private AuthService _authService;

        [TestInitialize]
        public void Setup()
        {
            var jwtSettings = Options.Create(new JwtSettings
            {
                Key = "djasçldfjsalkfnaslkdasdsadsavnad",
                Issuer = "Test",
                Audience = "TestAudience",
                ExpireMinutes = 5
            });

            var storageSettings = Options.Create(new StorageSettings
            {
                UsersFile = "users_test.json",
                Folder = "Data"
            });

            var filePath = Path.Combine("Data", "users_test.json");
            if (File.Exists(filePath))
                File.WriteAllText(filePath, "[]");

            _authService = new AuthService(jwtSettings, storageSettings);
        }

        [TestMethod]
        public void Register_Should_Create_User()
        {
            var result = _authService.Register(new RegisterRequest { Username = "dany", Password = "12345" });

            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void Login_Should_Fail_With_Invalid_User()
        {
            var result = _authService.Login(new LoginRequest { Username = "ghost", Password = "12345" });

            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public void Register_And_Login_Should_Succeed()
        {
            var register = _authService.Register(new RegisterRequest { Username = "user", Password = "12345" });
            Assert.IsTrue(register.IsSuccess);

            var login = _authService.Login(new LoginRequest { Username = "user", Password = "12345" });
            Assert.IsTrue(login.IsSuccess);
            Assert.IsNotNull(login.Content.Token);
        }
    }
}
