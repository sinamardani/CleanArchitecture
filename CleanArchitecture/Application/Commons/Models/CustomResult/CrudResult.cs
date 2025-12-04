using Domain.Commons.Enums;
using FluentValidation.Results;

namespace Application.Commons.Models.CustomResult;

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
        if (!string.IsNullOrWhiteSpace(message))
        {
            Messages.Add(new CrudMessage()
            {
                PropertyName = string.Empty,
                Message = message,
            });
        }
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
}