using Application.Commons.Models.CustomResult;
using MediatR;

namespace Application.Commons.Interfaces.Messaging.Query;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, CrudResult<TResponse>>
    where TQuery : IQuery<TResponse>
{

}