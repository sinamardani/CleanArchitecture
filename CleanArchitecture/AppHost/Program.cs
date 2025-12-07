using Aspire.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var web = builder.AddProject<Projects.Web>("web");

builder.Build().Run();
