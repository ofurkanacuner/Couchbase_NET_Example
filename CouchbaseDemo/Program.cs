using CouchbaseDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Couchbase services
builder.Services.AddSingleton<ICouchbaseClusterProvider>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration["Couchbase:ConnectionString"];
    var username = configuration["Couchbase:Username"];
    var password = configuration["Couchbase:Password"];
    var bucketName = configuration["Couchbase:BucketName"];

    return new CouchbaseClusterProvider(connectionString, username, password, bucketName);
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Couchbase Demo API",
        Version = "v1",
        Description = "API documentation for Couchbase integration with .NET Core"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
