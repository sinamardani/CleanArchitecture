using MediatR;
using Shared.Models.CustomResult;

namespace Application.Common.Interfaces.Messaging.Command;

public interface ICommand : IRequest<CrudResult>
{
    
}

public interface ICommand<TResponse> : IRequest<CrudResult<TResponse>>
{

}

