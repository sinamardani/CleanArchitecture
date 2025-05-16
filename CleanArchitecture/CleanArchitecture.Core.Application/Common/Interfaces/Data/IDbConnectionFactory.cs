using System.Data;

namespace CleanArchitecture.Core.Application.Common.Interfaces.Data;

public interface IDbConnectionFactory
{
    IDbConnection GetOpenConnection();
}