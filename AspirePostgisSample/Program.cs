var builder = DistributedApplication.CreateBuilder(args);
var postgres = builder.AddPostgres("postgres")
    .WithImage("postgis/postgis")
    .WithPgAdmin();
var postgisdb = postgres.AddDatabase("postgisdb");

builder.AddProject<Projects.AspirePostgisSample_API>("aspirepostgissample-api")
    .WithReference(postgisdb);
    

builder.Build().Run();
