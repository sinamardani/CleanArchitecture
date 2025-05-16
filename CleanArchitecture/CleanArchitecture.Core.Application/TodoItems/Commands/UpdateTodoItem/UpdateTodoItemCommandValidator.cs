using CleanArchitecture.Core.Application.TodoItems.Commands.CreateTodoItem;
using FluentValidation;

namespace CleanArchitecture.Core.Application.TodoItems.Commands.UpdateTodoItem;

public sealed class UpdateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    public UpdateTodoItemCommandValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(100).WithMessage("نمیتوانید بیشتر از 100 حرف را وارد نمایید")
            .NotEmpty().WithMessage("!نمیتوانید فیلد های اجباری را خالی بگذارید");
    }
}