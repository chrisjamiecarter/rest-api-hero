using System.Data;
using Npgsql;

namespace Movies.Application.Database;

public class NpgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly NpgsqlDataSource _dataSource;

    public NpgsqlConnectionFactory(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<IDbConnection> CreateConnectionAsync()
    {
        return await _dataSource.OpenConnectionAsync();
    }
}
