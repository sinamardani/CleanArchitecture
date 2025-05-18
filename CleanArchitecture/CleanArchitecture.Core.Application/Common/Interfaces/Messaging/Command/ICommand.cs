using CleanArchitecture.Core.Application.Common.Models.Results;
using MediatR;

namespace CleanArchitecture.Core.Application.Common.Interfaces.Messaging.Command;

public interface ICommand : IRequest<CrudResult>
{
    
}

public interface ICommand<TResponse> : IRequest<CrudResult<TResponse>>
{

}