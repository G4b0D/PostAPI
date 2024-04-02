using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DotnetAPI.Data;


public class DataContextDapper
{
    private readonly IConfiguration _config;
    public DataContextDapper(IConfiguration config){
        _config = config;
    }

    public async Task<IEnumerable<T>> LoadData<T>(string query)
    {
        IEnumerable<T> data;
        using (IDbConnection connection = new SqlConnection(GetConnectionString()))
        {
            data = await connection.QueryAsync<T>(query);
        }
        return data;
    }

    public async Task<IEnumerable<T>> LoadData<T>(string query, List<SqlParameter> parameters)
    {
        IEnumerable<T> data;
        var sqlParameters = new DynamicParameters();
        foreach (SqlParameter param in parameters)
        {
            sqlParameters.Add(param.ParameterName, param.Value);
        }
        using (IDbConnection connection = new SqlConnection(GetConnectionString()))
        {
            data = await connection.QueryAsync<T>(query, sqlParameters);
        }
        return data;
    }

    public async Task<T> LoadDataSingle<T>(string query)
    {
        IDbConnection dbConnection = new SqlConnection(GetConnectionString());
        return await dbConnection.QuerySingleAsync<T>(query);
    }

    public async Task<T> LoadDataSingle<T>(string query, List<SqlParameter> parameters)
    {
        var sqlParameters = new DynamicParameters();
        foreach (SqlParameter param in parameters)
        {
            sqlParameters.Add(param.ParameterName, param.Value);
        }
        using (IDbConnection connection = new SqlConnection(GetConnectionString()))
        {
            return await connection.QuerySingleAsync<T>(query, sqlParameters);
        }
    }

    public async Task<bool> ExecuteQuery(string query)
    {
        int rowsAffected;
        using (IDbConnection dbConnection = new SqlConnection(GetConnectionString()))
        {
            rowsAffected = await dbConnection.ExecuteAsync(query);
        }


        return rowsAffected > 0;
    }

    public async Task<bool> ExecuteQuery(string query, List<SqlParameter> parameters)
    {
        int rowsAffected;
        var sqlParameters = new DynamicParameters();
        foreach (SqlParameter param in parameters)
        {
            sqlParameters.Add(param.ParameterName, param.Value);
        }

        using (IDbConnection dbConnection = new SqlConnection(GetConnectionString()))
        {
            rowsAffected = await dbConnection.ExecuteAsync(query,sqlParameters);
        }

        return rowsAffected > 0;
    }

    public async Task<int> ExecuteQueryWithRowCount(string query)
    {
        int rowsAffected;
        using (IDbConnection dbConnection = new SqlConnection(GetConnectionString()))
        {
            rowsAffected = await dbConnection.ExecuteAsync(query);
        }
        return rowsAffected;
    }

    public async Task<int> ExecuteQueryWithRowCount(string query, List<SqlParameter> parameters)
    {
        int rowsAffected;
        var sqlParameters = new DynamicParameters();
        foreach (SqlParameter param in parameters)
        {
            sqlParameters.Add(param.ParameterName, param.Value);
        }

        using (IDbConnection dbConnection = new SqlConnection(GetConnectionString()))
        {
            rowsAffected = await dbConnection.ExecuteAsync(query, sqlParameters);
        }
        return rowsAffected;
    }

    #region Private Functions
    private string? GetConnectionString() 
    {
        return _config.GetConnectionString("DefaultConnection");
    }
    #endregion
}