using Application.Commons.Models.CustomResult;
using MediatR;

namespace Application.Commons.Interfaces.Messaging.Command;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, CrudResult>
    where TCommand : ICommand
{

}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, CrudResult<TResponse>>
    where TCommand : ICommand<TResponse>
{

}