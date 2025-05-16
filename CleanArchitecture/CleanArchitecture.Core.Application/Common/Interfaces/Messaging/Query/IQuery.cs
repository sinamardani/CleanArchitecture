using CleanArchitecture.Core.Application.Common.Models.Results;
using MediatR;

namespace CleanArchitecture.Core.Application.Common.Interfaces.Messaging.Query;

public interface IQuery<TResponse> : IRequest<CrudResult<TResponse>>
{
    
}