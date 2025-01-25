var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin();

var productdb = postgres.AddDatabase("productdb");

var keycloak = builder.AddKeycloak("keycloak", 8080)
    .WithRealmImport("Realms")
    .WithDataVolume();

var productservice = builder.AddProject<Projects.ProductService>("productservice")
    .WithReference(productdb)
    .WaitFor(productdb)
    .WithExternalHttpEndpoints();

builder.AddNpmApp("ShopFrontend", "../FrontendService")
    .WithReference(productservice)
    .WithReference(keycloak)
    .WaitFor(productservice)
    .WaitFor(keycloak)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.AddNpmApp("AdminFrontend", "../AdminFrontendService")
    .WithReference(productservice)
    .WithReference(keycloak)
    .WaitFor(productservice)
    .WaitFor(keycloak)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
