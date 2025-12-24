using MediatR;
using Shared.Models.CustomResult;

namespace Application.Common.Interfaces.Messaging.Command;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, CrudResult>
    where TCommand : ICommand
{

}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, CrudResult<TResponse>>
    where TCommand : ICommand<TResponse>
{

}

