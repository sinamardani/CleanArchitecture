using Application.Commons.Models.CustomResult;
using MediatR;

namespace Application.Commons.Interfaces.Messaging.Command;

public interface ICommand : IRequest<CrudResult>
{
    
}

public interface ICommand<TResponse> : IRequest<CrudResult<TResponse>>
{

}