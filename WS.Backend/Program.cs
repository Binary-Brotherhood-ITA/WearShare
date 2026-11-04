using MongoDB.Driver;
using WS.Backend.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Henter forbindelsesstrengen til MongoDB fra appsettings.
string? connectionString = builder.Configuration["MongoDb:ConnectionString"];

// Registrerer forbindelsen til MongoDB, så repositories kan bruge den.
builder.Services.AddSingleton<IMongoClient>(new MongoClient(connectionString));

// Gør det muligt for backend at modtage API-kald og vise Swagger.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// Registrerer vores to eksisterende repositories.
// CredentialsRepository er fjernet, da WearShare bruger login med navn uden adgangskode.
builder.Services.AddScoped<MemberRepository>();
builder.Services.AddScoped<ClothingRepository>();

// Understøttelse af OpenAPI.
builder.Services.AddOpenApi();

var app = builder.Build();

// Swagger er kun tilgængeligt, når backend kører i Development.
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