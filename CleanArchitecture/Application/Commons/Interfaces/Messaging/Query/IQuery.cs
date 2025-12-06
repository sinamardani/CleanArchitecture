using Application.Commons.Models.CustomResult;
using MediatR;

namespace Application.Commons.Interfaces.Messaging.Query;

public interface IQuery<TResponse> : IRequest<CrudResult<TResponse>>
{
    
}