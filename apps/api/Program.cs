using Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<PostgreSqlConnectionFactory>();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();
