var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder.AddSqlServer("sqlserver")
    .WithDataVolume();

var db = sqlServer.AddDatabase("CleanArchitectureDb");

builder.AddProject<Projects.Web>("web")
   .WithReference(db)
   .WaitFor(db);

builder.Build().Run();
