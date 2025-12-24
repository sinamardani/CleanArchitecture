using MediatR;
using Shared.Models.CustomResult;

namespace Application.Common.Interfaces.Messaging.Query;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, CrudResult<TResponse>>
    where TQuery : IQuery<TResponse>
{

}

