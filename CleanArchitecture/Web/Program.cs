using Application;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Web;
using Web.Extensions;
using Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

#if (UseAspire)
builder.AddServiceDefaults();
#endif

builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddWebServices();

var app = builder.Build();

await app.InitializeDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseOpenApi();
    app.UseSwaggerUi(setting => setting.Path = "/swagger");
    app.UseReDoc(setting => setting.Path = "/document");
}

app.UseHttpsRedirection();
#if (UseAspire)
app.MapDefaultEndpoints();
#endif

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


app.MapEndpoints();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();

public partial class Program;