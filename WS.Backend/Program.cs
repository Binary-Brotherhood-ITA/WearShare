using MongoDB.Driver;
using WS.Backend.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Henter vores connection string fra appsettings
var connectionString = builder.Configuration["MongoDb:ConnectionString"];

// Registrerer MongoDB-forbindelsen, så vores repositories kan bruge den
builder.Services.AddSingleton<IMongoClient>(new MongoClient(connectionString));

//Forklaring:
// "builder.Configuration": Læser indstillingerne fra vores appsettings.
// "MongoDb:ConnectionString": Finder adressen til MongoDB Atlas.
// "MongoClient	Opretter klienten" som bruges til at kommunikere med MongoDB.
// "AddSingleton" Genbruger den samme MongoDB-klient i vores backend.

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// Registrerer MemberRepository, så vores Controller kan bruge den
builder.Services.AddScoped<MemberRepository>();
//AddScoped betyder, at ASP.NET Core opretter og genbruger én repository-instans inden for samme HTTP-request.


// Registrerer ClothingRepository, så ClothingController kan bruge den
builder.Services.AddScoped<ClothingRepository>();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();