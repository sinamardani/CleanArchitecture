using Application.Features.TodoItems.Queries.GetTodoItemsWithPagination;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Unit.Application.TodoItems.Queries;

public class GetTodoItemsWithPaginationQueryValidatorTests
{
    private readonly GetTodoItemsWithPaginationQueryValidator _validator = new();

    [Fact]
    public void Validate_WithValidQuery_ShouldPass()
    {
        var query = new GetTodoItemsWithPaginationQuery
        {
            ListId = 1,
            PageNumber = 1,
            PageSize = 10
        };

        var result = _validator.TestValidate(query);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyListId_ShouldFail()
    {
        var query = new GetTodoItemsWithPaginationQuery
        {
            ListId = 0,
            PageNumber = 1,
            PageSize = 10
        };

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.ListId);
    }

    [Fact]
    public void Validate_WithPageNumberLessThanOne_ShouldFail()
    {
        var query = new GetTodoItemsWithPaginationQuery
        {
            ListId = 1,
            PageNumber = 0,
            PageSize = 10
        };

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.PageNumber);
    }

    [Fact]
    public void Validate_WithPageSizeLessThanOne_ShouldFail()
    {
        var query = new GetTodoItemsWithPaginationQuery
        {
            ListId = 1,
            PageNumber = 1,
            PageSize = 0
        };

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }

    [Fact]
    public void Validate_WithValidPagination_ShouldPass()
    {
        var query = new GetTodoItemsWithPaginationQuery
        {
            ListId = 1,
            PageNumber = 2,
            PageSize = 20
        };

        var result = _validator.TestValidate(query);

        result.ShouldNotHaveAnyValidationErrors();
    }
}

