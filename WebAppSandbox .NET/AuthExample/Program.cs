var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configures the ASP.NET Core backend to use the config, allow access from the Angular client, etc...
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDevClient",
      builder =>
      {
          builder
          .WithOrigins("http://localhost:4200")
          .AllowAnyHeader()
          .AllowAnyMethod();
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

// Use configuration policy set up in services to allow client
app.UseCors("AllowAngularDevClient");

app.MapControllers();

app.Run();
