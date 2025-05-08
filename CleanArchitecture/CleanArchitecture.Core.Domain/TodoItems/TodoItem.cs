﻿using CleanArchitecture.Core.Domain.Common;
using CleanArchitecture.Core.Domain.TodoItems.Enums;
using CleanArchitecture.Core.Domain.TodoItems.Events;
using CleanArchitecture.Core.Domain.TodoLists;

namespace CleanArchitecture.Core.Domain.TodoItems;

public sealed class TodoItem : BaseAuditTableEntity
{
    public int ListId { get; set; }

    public string? Title { get; set; }

    public string? Note { get; set; }

    public PriorityLevel Priority { get; set; }

    public DateTime? Reminder { get; set; }

    private bool _done;
    public bool Done
    {
        get => _done;
        set
        {
            if (value && !_done)
            {
                AddDomainEvent(new TodoItemCompletedEvent(this));
            }

            _done = value;
        }
    }
    public TodoList List { get; set; } = null!;
}