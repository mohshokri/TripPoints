using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TripPoints.Infrastructure;
using TripPoints.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/////////
builder.Services.AddDbContext<TripDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("Data Source=.;Initial Catalog=TripDbContext;Trusted_Connection=True;TrustServerCertificate=True;")));
//builder.Services.AddDbContext<TripDbContext>(options =>
//            options.UseSqlServer(builder.Configuration.GetConnectionString("Server=(localdb)\\MSSQLLocalDB;Database=TripDbContext;Trusted_Connection=True;TrustServerCertificate=true;")));
builder.Services.AddScoped<TripService>();

builder.Services.AddControllers();
/////////

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
