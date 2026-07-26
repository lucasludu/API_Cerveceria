var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("RedisConnection");

var api = builder.AddProject<Projects.WebApi>("webapi")
                 .WithReference(cache);

builder.AddViteApp("frontend-app", @"..\..\Cerveceria")
       .WithReference(api)
       .WithHttpEndpoint(env: "PORT");

builder.Build().Run();

