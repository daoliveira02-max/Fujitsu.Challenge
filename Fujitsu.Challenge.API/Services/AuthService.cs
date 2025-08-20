using Fujitsu.Challenge.API.Interfaces;
using Fujitsu.Challenge.API.Models;
using Fujitsu.Challenge.API.Models.Auth;
using Fujitsu.Challenge.API.Models.Settings;
using Fujitsu.Challenge.API.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Fujitsu.Challenge.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IFileRepository<User> _fileRepository;

        public AuthService(IOptions<JwtSettings> jwtSettings, IOptions<StorageSettings> storageSettings)
        {
            _jwtSettings = jwtSettings.Value;
            _fileRepository = new FileRepository<User>(storageSettings.Value.UsersFile, storageSettings.Value.Folder);
        }

        public IResponse<LoginResponse> Login(LoginRequest request)
        {
            var username = request.Username?.Trim().ToLowerInvariant();

            var users = _fileRepository.GetAll();
            var user = users.FirstOrDefault(u => u.Username == username);

            if (user == null)
                return Response<LoginResponse>.Failure("User does not exist!");

            if (user.PasswordHash != PasswordHash(request.Password))
                return Response<LoginResponse>.Failure("Invalid Password!");

            return Response<LoginResponse>.Success(GenerateJwtToken(user));
        }

        public IResponse<bool> Register(RegisterRequest request)
        {
            var username = request.Username?.Trim().ToLowerInvariant();

            var users = _fileRepository.GetAll();

            if (users.Any(u => u.Username == username))
                return Response<bool>.Failure("User already exists!");

            var newUser = new User
            {
                Id = users.Any() ? users.Max(u => u.Id) + 1 : 1,
                Username = username,
                PasswordHash = PasswordHash(request.Password)
            };

            users.Add(newUser);
            _fileRepository.SaveAll(users);

            return Response<bool>.Success(true);
        }

        private LoginResponse GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginResponse
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = tokenDescriptor.Expires!.Value
            };
        }

        private string PasswordHash(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
