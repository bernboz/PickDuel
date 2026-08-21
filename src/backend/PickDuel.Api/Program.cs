using Microsoft.EntityFrameworkCore;
using PickDuel.Infrastructure.Data;
using PickDuel.Infrastructure.Repositories;
using PickDuel.Application.Repositories.Interfaces;
using PickDuel.Application.Mappers.Interfaces;
using PickDuel.Application.Mappers;


var builder = WebApplication.CreateBuilder(args);


// Database configuration
builder.Services.AddDbContext<PickDuelDbContext>(
    options =>
    {
        options.UseNpgsql(
            builder.Configuration.GetConnectionString(
                "DefaultConnection"
            )
        );
    });


// Repository registrations
builder.Services.AddScoped<
    IPickRepository,
    PickRepository>();


// OpenAPI
builder.Services.AddOpenApi();

//UserMapper registration
builder.Services.AddScoped<IUserMapper, UserMapper>();


var app = builder.Build();


// Configure HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();


app.Run();