using CleanArchitecture.Core.Application.Common.Models.Results;
using MediatR;

namespace CleanArchitecture.Core.Application.Common.Interfaces.Messaging.Query;

// ReSharper disable once TypeParameterCanBeVariant
public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, CrudResult<TResponse>>
    where TQuery : IQuery<TResponse>
{

}