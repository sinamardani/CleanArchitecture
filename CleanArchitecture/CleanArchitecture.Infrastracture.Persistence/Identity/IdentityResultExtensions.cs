using CleanArchitecture.Core.Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Persistence.Identity;

public static class IdentityResultExtensions
{
    public static Result ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(result.Errors.Select(e => e.Description));
    }
}