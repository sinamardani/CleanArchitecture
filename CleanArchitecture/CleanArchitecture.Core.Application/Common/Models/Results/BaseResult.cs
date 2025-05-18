﻿using CleanArchitecture.Core.Domain.Common.Enum;

namespace CleanArchitecture.Core.Application.Common.Models.Results;

public class BaseResult
{
    public CrudStatus Status { get; set; }
    public List<CrudMessage> Messages { get; set; } = new();

    public string Message
    {
        get => Messages.Any() ? string.Join("-", Messages.Select(c => c.Message.ToString())) : Status.ToString();
        set => Messages.Add(new CrudMessage() { PropertyName = string.Empty, Message = value });
    }

    public bool Succeeded()
    {
        return Status == CrudStatus.Succeeded;
    }

    public bool Failed()
    {
        return Status != CrudStatus.Succeeded;
    }
}