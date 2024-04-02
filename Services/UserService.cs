using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.Data.SqlClient;
using System.Reflection;
using System;
using DotnetAPI.DTOs;

namespace DotnetAPI.Services
{
    public class UserService
    {
        private readonly DataContextDapper _dapper;

        public UserService(IConfiguration configuration)
        {
            _dapper = new DataContextDapper(configuration);
        }

        public async Task<bool> CreateUser(UserDto user)
        {
            string query = Queries.CreateUserQuery;
            List<SqlParameter> parameters =
            [
                new SqlParameter("@FirstName", user.FirstName),
                new SqlParameter("@LastName", user.LastName),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@Gender",user.Gender),
            ];

            

            return await _dapper.ExecuteQuery(query, parameters);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            string query = Queries.GetAllUsersQuery;
            return await _dapper.LoadData<User>(query);
        }

        public async Task<User> GetUserWithId(int userId)
        {
            string query = Queries.GetSingleUserQuery;
            List<SqlParameter> parameters = [new SqlParameter("@UserId", userId)];
            return await _dapper.LoadDataSingle<User>(query, parameters);
        }

        public async Task<bool> UpdateUser(UserToEdit user)
        {
            User originalUser = await GetUserWithId(user.UserId);
            if ( originalUser == null) { return false; }
            List<SqlParameter> parameters =
                [
                    new SqlParameter("@Email",user.Email ?? originalUser.Email),
                    new SqlParameter("@Gender",user.Gender ?? originalUser.Gender),
                    new SqlParameter("@Active",user.Active ?? originalUser.Active ),
                    new SqlParameter("@UserId",user.UserId)
                ];
            string query = Queries.UpdateUserQuery;
            return await _dapper.ExecuteQuery(query, parameters);
        }

        public async Task<bool> DeleteUser(int userId)
        {
            string query = Queries.DeleteUserQuery;
            List<SqlParameter> parameters = [new SqlParameter("@UserId",userId)];
            return await _dapper.ExecuteQuery(query,parameters);
        }

    }
}
