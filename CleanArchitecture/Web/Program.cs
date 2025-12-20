using Application;
using Infrastructure;
using Persistence;
using ServiceDefaults;
using Web;
using Web.Extensions;
using Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddWebServices(builder.Configuration, builder.Environment);

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
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultEndpoints();
app.UseGlobalExceptionHandler();
app.MapEndpoints();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();

public partial class Program;