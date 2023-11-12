using aspnetbackend;
using Elfie.Serialization;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json.Linq;

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
app.UseStaticFiles();
app.UseSwagger();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

async void ReadFirebaseAdminSDK()
{
    if (FirebaseMessaging.DefaultInstance == null)
    {
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromJson(File.ReadAllText("C:\\Users\\adamw\\source\\repos\\GuardianEyeBackend\\Resources\\Raw\\guardianeye-29abb-firebase-adminsdk-ia13w-0dfbc93058.json"))
        });
    }
}