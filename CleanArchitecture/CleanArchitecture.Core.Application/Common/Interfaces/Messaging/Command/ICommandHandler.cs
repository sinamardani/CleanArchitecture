using CleanArchitecture.Core.Application.Common.Models.Results;
using MediatR;

namespace CleanArchitecture.Core.Application.Common.Interfaces.Messaging.Command;

// ReSharper disable once TypeParameterCanBeVariant
public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, CrudResult>
    where TCommand : ICommand
{

}

// ReSharper disable once TypeParameterCanBeVariant
public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, CrudResult<TResponse>>
    where TCommand : ICommand<TResponse>
{

}