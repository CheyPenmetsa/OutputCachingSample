using BusinessLogic.Data;

var builder = WebApplication.CreateBuilder(args);

// This will load the MongoDb settings
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection(nameof(MongoDbSettings)));

// Add services to the container.
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Caching
builder.Services.AddOutputCache(options =>
{
    // By default all the outputs will be cached for 30 seconds
    options.AddBasePolicy(builder =>
        builder.Expire(TimeSpan.FromSeconds(30)));

    options.AddPolicy("CacheByCity", builder =>
    {
        builder.Expire(TimeSpan.FromSeconds(10))
        .SetVaryByQuery("city");
    });

    ////Which ever endpoint using this policy that paritcular endpoint will be cached for 10 seconds
    //options.AddPolicy("CacheForTenSeconds", builder =>
    //    builder.Expire(TimeSpan.FromSeconds(10))
    //                .SetVaryByQuery("city"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.UseOutputCache();

app.Run();
