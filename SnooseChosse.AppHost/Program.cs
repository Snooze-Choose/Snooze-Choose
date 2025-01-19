var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin();

var productdb = postgres.AddDatabase("productdb");

builder.AddProject<Projects.ProductService>("productservice")
    .WithReference(productdb)
    .WaitFor(productdb);

builder.Build().Run();
