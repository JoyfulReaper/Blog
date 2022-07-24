/*
MIT License

Copyright(c) 2022 Kyle Givler
https://github.com/JoyfulReaper

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

/*
 * This is a lightly modified version of Tim Corey's Data access code
 * From the TimCoRetail Manager series.
 */

using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace BlogApi.Library.DataAccess;
public class SqlDataAccess : IDataAccess
{
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;
    private bool isClosed = false;
    private readonly IConfiguration _config;
    private readonly ILogger<SqlDataAccess> _logger;

    public SqlDataAccess(IConfiguration config,
        ILogger<SqlDataAccess> logger)
    {
        _config = config;
        _logger = logger;
    }

    public string GetConnectionString(string name)
    {
        return _config.GetConnectionString(name);
    }

    public async Task<List<T>> LoadDataAsync<T, U>(string storedProcedure, U parameters, string connectionStringName)
    {
        string connectionString = GetConnectionString(connectionStringName);

        using (IDbConnection connection = new SqlConnection(connectionString))
        {
            List<T> rows = (await connection.QueryAsync<T>(storedProcedure, parameters,
                commandType: CommandType.StoredProcedure))
                .ToList();

            return rows;
        }
    }

    public async Task SaveDataAsync<T>(string storedProcedure, T parameters, string connectionStringName)
    {
        string connectionString = GetConnectionString(connectionStringName);

        using (IDbConnection connection = new SqlConnection(connectionString))
        {
            await connection.ExecuteAsync(storedProcedure, parameters,
                commandType: CommandType.StoredProcedure);
        }
    }

    public async Task<int> SaveDataAndGetIdAsync<T>(string storedProcedure, T parameters, string connectionStringName)
    {
        string connectionString = GetConnectionString(connectionStringName);

        using (IDbConnection connection = new SqlConnection(connectionString))
        {
            int id = await connection.QuerySingleAsync<int>(storedProcedure, parameters,
                commandType: CommandType.StoredProcedure);

            return id;
        }
    }

    public async Task SaveDataInTransactionAsync<T>(string storedProcedure, T parameters)
    {
        await _connection.ExecuteAsync(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure, transaction: _transaction);
    }

    public async  Task<List<T>> LoadDataInTransactionAsync<T, U>(string storedProcedure, U parameters)
    {
        List<T> rows = (await _connection.QueryAsync<T>(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure, transaction: _transaction))
            .ToList();

        return rows;
    }

    public void StartTransaction(string connectionStringName)
    {
        string connectionString = GetConnectionString(connectionStringName);
        _connection = new SqlConnection(connectionString);
        _connection.Open();
        _transaction = _connection.BeginTransaction();

        isClosed = false;
    }

    public void CommitTransaction()
    {
        _transaction?.Commit();
        _connection?.Close();

        isClosed = true;
    }

    public void RollbackTransaction()
    {
        _transaction?.Rollback();
        _connection?.Close();

        isClosed = true;
    }

    public void Dispose()
    {
        if (isClosed == false)
        {
            try
            {
                CommitTransaction();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Commit transaction failed in the Dispose() method.");
            }
        }

        _transaction = null;
        _connection = null;
    }

    public async Task<List<T>> QueryRawSql<T, U>(string sql, U parameters, string connectionStringName)
    {
        string connectionString = GetConnectionString(connectionStringName);
        using IDbConnection connection = new SqlConnection(connectionString);

        return (await connection.QueryAsync<T>(sql, parameters,
            commandType: CommandType.Text))
            .ToList();
    }

    public async Task ExecuteRawSql<T>(string sql, T parameters, string connectionStringName)
    {
        string connectionString = GetConnectionString(connectionStringName);
        using IDbConnection connection = new SqlConnection(connectionString);

        await connection.ExecuteAsync(sql, parameters,
            commandType: CommandType.Text);
    }
}