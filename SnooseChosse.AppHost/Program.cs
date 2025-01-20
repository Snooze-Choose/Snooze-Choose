var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin();

var productdb = postgres.AddDatabase("productdb");

var productservice = builder.AddProject<Projects.ProductService>("productservice")
    .WithReference(productdb)
    .WaitFor(productdb)
    .WithExternalHttpEndpoints();

builder.AddNpmApp("vue", "../FrontendService")
    .WithReference(productservice)
    .WaitFor(productservice)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();



builder.Build().Run();



builder.Build().Run();
