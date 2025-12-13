using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Web.Extensions;

public static class ExceptionHandlerExtensions
{
    public static WebApplication UseGlobalExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = exceptionFeature?.Error;

                var problem = new ProblemDetails
                {
                    Title = "An unexpected error occurred",
                    Detail = exception?.Message,
                    Status = StatusCodes.Status500InternalServerError,
                    Instance = context.Request.Path
                };

                context.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsJsonAsync(problem);
            });
        });

        return app;
    }
}

