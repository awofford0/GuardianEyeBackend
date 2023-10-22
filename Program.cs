using aspnetbackend;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<ForecastContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Weather")));
builder.Services.AddDbContext<DetectionContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Detection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDirectoryBrowser();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
