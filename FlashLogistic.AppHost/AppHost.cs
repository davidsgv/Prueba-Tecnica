var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sqlserver")
    .WithDataVolume("FlashLogisticsSQL")
    .WithLifetime(ContainerLifetime.Persistent);

var db = sql.AddDatabase("sqldata");

var apiService = builder.AddProject<Projects.FlashLogistic_ApiService>("apiservice")
    .WithReference(db)
    .WaitFor(db)
    .WithHttpHealthCheck("/health");

//builder.AddProject<Projects.FlashLogistic_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithHttpHealthCheck("/health")
//    .WithReference(apiService)
//    .WaitFor(apiService);

builder.Build().Run();
