var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ProductService>("productservice");

builder.Build().Run();
