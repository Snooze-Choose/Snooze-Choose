var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin();

var productdb = postgres.AddDatabase("productdb");

var productservice = builder.AddProject<Projects.ProductService>("productservice")
    .WithReference(productdb)
    .WaitFor(productdb)
    .WithExternalHttpEndpoints();

builder.AddNpmApp("reactvite", "../frontendservice")
    .WithReference(productservice)
    .WithEnvironment("BROWSER", "none")
    .WithHttpEndpoint(env: "VITE_PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
