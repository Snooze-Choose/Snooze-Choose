var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin();

var productdb = postgres.AddDatabase("productdb");
var orderdb = postgres.AddDatabase("orderdb");

var keycloak = builder.AddKeycloak("keycloak", 8080)
    .WithDataVolume();

var productservice = builder.AddProject<Projects.ProductService>("productservice")
    .WithReference(productdb)
    .WithReference(keycloak)
    .WaitFor(keycloak)
    .WaitFor(productdb)
    .WithExternalHttpEndpoints();

var orderservice = builder.AddProject<Projects.OrderService>("orderservice")
    .WithReference(orderdb)
    .WithReference(keycloak)
    .WaitFor(keycloak)
    .WaitFor(orderdb)
    .WithExternalHttpEndpoints();

builder.AddNpmApp("shopfrontend", "../FrontendService")
    .WithReference(productservice)
    .WithReference(orderservice)
    .WithReference(keycloak)
    .WaitFor(productservice)
    .WaitFor(keycloak)
    .WaitFor(orderservice)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.AddNpmApp("adminfrontend", "../AdminFrontendService")
    .WithReference(productservice)
    .WithReference(orderservice)
    .WithReference(keycloak)
    .WaitFor(productservice)
    .WaitFor(keycloak)
    .WaitFor(orderservice)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();


builder.Build().Run();
