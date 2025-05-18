using CleanArchitecture.Core.Application;
using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.Infrastructure.Resources;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Web;
using Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddResourcesInfrastructure();
builder.Services.AddWebService();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
    app.UseHealthChecks("/health");
    app.MapHealthChecks("/alive", new HealthCheckOptions
    {
        Predicate = r => r.Tags.Contains("live"),
    });
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseOpenApi();
app.UseSwaggerUi(settings =>
{
    settings.Path = "/swagger";
});


app.UseExceptionHandler(options => { });
app.Map("/", () => Results.Redirect("/swagger"));
app.MapEndpoints();


app.Run();

public partial class Program
{
}