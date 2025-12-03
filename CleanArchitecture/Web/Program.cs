using Application;
using Persistence;
using Web;
using Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

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

app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();

public partial class Program;