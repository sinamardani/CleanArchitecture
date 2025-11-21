using Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebServices();

var app = builder.Build();

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