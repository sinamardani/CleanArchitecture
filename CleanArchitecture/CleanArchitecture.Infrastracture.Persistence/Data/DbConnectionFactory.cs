using System.Data;
using CleanArchitecture.Core.Application.Common.Interfaces.Data;
using Microsoft.Data.SqlClient;

namespace CleanArchitecture.Infrastructure.Persistence.Data;

public sealed class DbConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public IDbConnection GetOpenConnection()
    {
        IDbConnection connection = new SqlConnection(connectionString);
        connection.Open();

        return connection;
    }
}