using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using WS.Backend.Models;

namespace WS.Backend.Repositories;

public class CredentialsRepository
{
    // Gemmer en reference til loginoplysninger i MongoDB
    private readonly IMongoCollection<MemberCredentials> _credentials;

    // Bruges til at hashe adgangskoder
    private readonly PasswordHasher<MemberCredentials> _passwordHasher = new();

    // Constructor - forbinder repository med MongoDB
    public CredentialsRepository(
        IMongoClient mongoClient,
        IConfiguration configuration)
    {
        // Henter databasenavnet fra appsettings
        string databaseName =
            configuration["MongoDb:DatabaseName"] ?? "WearShareDb";

        // Finder vores database
        IMongoDatabase database =
            mongoClient.GetDatabase(databaseName);

        // Finder collection til loginoplysninger
        _credentials =
            database.GetCollection<MemberCredentials>("MemberCredentials");
    }

    // Opretter loginoplysninger til et medlem
    public bool CreateCredentials(int memberId, string email, string password)
    {
        // Gør email ensartet, så store og små bogstaver behandles ens
        string normalizedEmail = email.Trim().ToLowerInvariant();

        // Kontrollerer om email allerede findes
        MemberCredentials? existingCredentials =
            _credentials.Find(c => c.Email == normalizedEmail)
                .FirstOrDefault();

        // Stopper hvis email allerede er registreret
        if (existingCredentials != null)
        {
            return false;
        }

        // Opretter objektet med medlemmets oplysninger
        MemberCredentials credentials = new MemberCredentials
        {
            MemberId = memberId,
            Email = normalizedEmail
        };

        // Omdanner adgangskoden til en sikker hash
        credentials.PasswordHash =
            _passwordHasher.HashPassword(credentials, password);

        // Gemmer oplysningerne i MongoDB
        // Den almindelige adgangskode bliver ikke gemt
        _credentials.InsertOne(credentials);

        // Returnerer true når oplysningerne er gemt
        return true;
    }
}
