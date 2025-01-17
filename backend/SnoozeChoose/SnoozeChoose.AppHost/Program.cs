var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Product_API>("product-api");

builder.Build().Run();
