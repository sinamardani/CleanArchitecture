using CleanArchitecture.Core.Domain.Common.Enum;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Core.Application.Common.Models.Results;

public class CrudResult<T> : BaseResult
{
    public T Result { get; set; } = default!;

    public CrudResult()
    {
        Status = CrudStatus.Succeeded;
    }

    public CrudResult(CrudStatus status, string message, string key = "")
    {
        Status = status;
        Messages.Add(new CrudMessage()
        {
            PropertyName = key,
            Message = message,
        });
    }

    public CrudResult(CrudStatus status)
    {
        Status = status;
    }

    public CrudResult(CrudStatus status, T model, string message = "")
    {
        Result = model;
        Status = status;
        if (string.IsNullOrWhiteSpace(message)) message = message;
    }

    public CrudResult(IEnumerable<IdentityError> errors)
    {
        Status = CrudStatus.IdentityError;
        Messages = errors.Select(s => new CrudMessage()
        {
            PropertyName = s.Code,
            Message = s.Description
        }).ToList();
    }
}

public class CrudResult : BaseResult
{
    public CrudResult()
    {
        Status = CrudStatus.Failed;
    }

    public CrudResult(CrudStatus status)
    {
        Status = status;
    }

    public CrudResult(CrudStatus status, string message, string key = "")
    {
        Status = status;
        Messages = new List<CrudMessage>()
        {
            new()
            {
                PropertyName = key,
                Message = message,
            }
        };
    }

    public CrudResult(IEnumerable<ValidationFailure> failures)
    {
        Status = CrudStatus.InputNotValid;
        Messages = failures.Select(s => new CrudMessage()
        {
            PropertyName = s.PropertyName,
            Message = s.ErrorMessage,
        }).ToList();
    }

    public CrudResult(IEnumerable<IdentityError> errors)
    {
        Status = CrudStatus.InputNotValid;
        Messages = errors.Select(s => new CrudMessage()
        {
            PropertyName = s.Code,
            Message = s.Description,
        }).ToList();
    }
}