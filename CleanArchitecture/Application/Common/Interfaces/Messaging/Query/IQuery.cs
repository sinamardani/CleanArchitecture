using MediatR;
using Shared.Models.CustomResult;

namespace Application.Common.Interfaces.Messaging.Query;

public interface IQuery<TResponse> : IRequest<CrudResult<TResponse>>
{
    
}

