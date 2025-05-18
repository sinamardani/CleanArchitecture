using CleanArchitecture.Core.Application.Common.Models.Results;
using CleanArchitecture.Core.Domain.Common.Enum;
using FluentValidation;
using MediatR;
using ValidationException = CleanArchitecture.Core.Application.Common.Exceptions.ValidationException;

namespace CleanArchitecture.Core.Application.Common.Behaviours;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : BaseResult, new()
{


    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResult =
                await Task.WhenAll(validators
                    .Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResult
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .Select(s => new CrudMessage()
                {
                    Message = s.ErrorMessage,
                    PropertyName = s.PropertyName
                }).ToList();

            if (failures.Any())
                return new TResponse
                {
                    Status = CrudStatus.InputNotValid,
                    Messages = failures
                };
        }

        return await next(cancellationToken);
    }
}