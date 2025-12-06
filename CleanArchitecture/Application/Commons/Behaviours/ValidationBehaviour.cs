using Application.Commons.Models.CustomResult;
using Domain.Commons.Enums;
using FluentValidation;
using MediatR;

namespace Application.Commons.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Any())
        {
            var responseType = typeof(TResponse);
            
            if (responseType == typeof(CrudResult))
            {
                var crudResult = new CrudResult(failures);
                return (TResponse)(object)crudResult;
            }
            
            if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(CrudResult<>))
            {
                var genericArg = responseType.GetGenericArguments()[0];
                var crudResultType = typeof(CrudResult<>).MakeGenericType(genericArg);
                var crudResult = Activator.CreateInstance(crudResultType, CrudStatus.InputNotValid);
                
                if (crudResult is BaseResult baseResult)
                {
                    baseResult.Messages = failures.Select(f => new CrudMessage
                    {
                        PropertyName = f.PropertyName,
                        Message = f.ErrorMessage
                    }).ToList();
                }
                
                return (TResponse)crudResult!;
            }
            throw new ValidationException(failures);
        }

        return await next();
    }
}

