var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder.AddSqlServer("sqlserver")
    .WithEndpoint("tcp", endpoint =>
    {
        endpoint.Port = 63295;
        endpoint.IsProxied = false;
    })
    .WithDataVolume();

var redis = builder.AddRedis("redis")
    .WithEndpoint("tcp", endpoint =>
    {
        endpoint.Port = 6379;
        endpoint.IsProxied = false;
    });

var db = sqlServer.AddDatabase("CleanArchitectureDb");
var loggingDb = sqlServer.AddDatabase("LoggingDb");


builder.AddProject<Projects.Web>("web")
   .WithReference(db)
   .WithReference(redis)
   .WithReference(loggingDb)
   .WaitFor(db);

builder.Build().Run();
