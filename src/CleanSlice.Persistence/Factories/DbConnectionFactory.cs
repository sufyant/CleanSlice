using System.Data;
using CleanSlice.Application.Abstractions.Data;
using Npgsql;

namespace CleanSlice.Persistence.Factories;

internal sealed class DbConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        return connection;
    }
}
