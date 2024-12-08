using BeSpokedBikesAPI.DataAccess;
using BeSpokedBikesAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BeSpokedBikesSQLContainer>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("BeSpokedBikesSQLDB")));

builder.Services.AddScoped<IBeSpokeBikesRepo,  BeSpokeBikesRepo>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        options.AddPolicy("AllowSpecificOrigin",
         policy => policy.WithOrigins("http://localhost:4200") // Your client app's URL
                         .AllowAnyHeader()
                         .AllowAnyMethod());
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

app.UseCors("AllowSpecificOrigin");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BeSpokedBikesSQLContainer>();
    dbContext.Database.EnsureCreated();
}

app.Run();
