var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder.AddSqlServer("sqlserver")
    .WithDataVolume();

var db = sqlServer.AddDatabase("CleanArchitectureDb");

var redis = builder.AddRedis("redis")
    .WithEndpoint("tcp", e => e.Port = 6379);

builder.AddProject<Projects.Web>("web")
   .WithReference(db)
   .WithReference(redis)
   .WaitFor(db);

builder.Build().Run();
