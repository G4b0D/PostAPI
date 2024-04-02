using DotnetAPI.Data;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using DotNetAPI_EF.Dtos;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DotnetAPI.Util
{
    public class AuthService
    {
        private readonly IConfiguration _config;
        private readonly DataContextDapper _dapper;
        public AuthService(IConfiguration configuration)
        {
            _config = configuration;
            _dapper = new DataContextDapper(configuration);

        }

        /// <summary>
        /// Generates Random Password Salt
        /// </summary>
        /// <returns>Password Salt</returns>
        public byte[] GeneratePasswordSalt()
        {
            byte[] passwordSalt = new byte[128 / 8];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(passwordSalt);
            }
            return passwordSalt;
        }

        /// <summary>
        /// Generates random password hash using salt.
        /// </summary>
        /// <param name="password">User defined password.</param>
        /// <param name="passwordSalt">Generated Password Salt.</param>
        /// <returns>Password Hash</returns>
        /// <remarks>
        /// This function uses HMACSHA256.
        /// </remarks>
        public byte[] GetPasswordHash(string password, byte[] passwordSalt)
        {
            string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value + Convert.ToBase64String(passwordSalt);
            byte[] passwordHash = KeyDerivation.Pbkdf2(
                    password: password,
                    salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8

            );
            return passwordHash;
        }

        /// <summary>
        /// Creates JWT token for user.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>JWT token</returns>
        public string CreateToken(int userId)
        {
            Claim[] claims =
            [
                new Claim("userId",userId.ToString())
            ];

            string? tokenKeyString = _config.GetSection("AppSettings:TokenKey").Value;
            SymmetricSecurityKey tokenKey = new(Encoding.UTF8.GetBytes(tokenKeyString != null ? tokenKeyString : string.Empty));
            SigningCredentials credentials = new(tokenKey, SecurityAlgorithms.HmacSha256);

            SecurityTokenDescriptor descriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddDays(1)
            };

            JwtSecurityTokenHandler tokenHandler = new();

            SecurityToken token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);

        }

        public async Task<int> GetUserId(string email)
            ///
        {
            string query = Queries.GetUserIdQuery;
            List<SqlParameter> parameters = [new SqlParameter("@Email", email)];
            int userId = await _dapper.LoadDataSingle<int>(query, parameters);
            return userId;
        }

        /// <summary>
        /// Returns userId if provided userId matches in db.
        /// </summary>
        public async Task<int> GetValidatedUserId(string userId)
        {
            string query = Queries.ValidateUserIdQuery;
            List<SqlParameter> parameters = [new SqlParameter("@UserId", userId)];
            int validatedUserId = await _dapper.LoadDataSingle<int>(query, parameters);
            return validatedUserId;
        }

        /// <summary>
        /// Checks if User is already in db. Returns true if user found
        /// </summary>
        public async Task<bool> UserExists(string userEmail)
        {
            string query = Queries.UserExistsQuery;
            List<SqlParameter> parameters = [new SqlParameter ("@Email", userEmail)];
            IEnumerable<string> existingUsers = await _dapper.LoadData<string>(query,parameters);

            if (existingUsers.Any())
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CreateAuthUser(string userEmail, byte[] passwordSalt, byte[] passwordHash)
        {
            string query = Queries.RegisterAuthUserQuery;

            List<SqlParameter> parameters =
            [
                new SqlParameter("@Email",userEmail),
                new SqlParameter("@PasswordSalt",passwordSalt),
                new SqlParameter("@PasswordHash",passwordHash)
            ];

            if (! await _dapper.ExecuteQuery(query, parameters))
            {
                return false;
            }
            return true;
        }
        public async Task<bool> CreateUser(UserForRegistrationDto user)
        {
            string query = Queries.CreateUserQuery;
            List<SqlParameter> parameters =
            [
                new SqlParameter("@FirstName", user.FirstName),
                new SqlParameter("@LastName", user.LastName),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@Gender",user.Gender),
            ];

            if (! await _dapper.ExecuteQuery(query, parameters))
            {
                throw new Exception("Error registering user!");
            }
            return true;
        }

        public async Task<bool> ValidatePassword(string email, string password)
        {
            string query = Queries.GetSaltandHashQuery;
            List<SqlParameter> parameters =
                [
                    new SqlParameter("@Email",email)
                ];
            UserForLoginConfirmationDto userConfirmation = await _dapper.LoadDataSingle<UserForLoginConfirmationDto>(query,parameters);
            if (userConfirmation == null)
            {
                throw new Exception("email not found");
            }
            byte[] passwordHash = GetPasswordHash(password, userConfirmation.PasswordSalt);
            if (!userConfirmation.PasswordHash.SequenceEqual(passwordHash))
            {
                return false;
            }
            return true;
        }
    }
}
