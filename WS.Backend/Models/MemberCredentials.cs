using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WS.Backend.Models;

public class MemberCredentials
{
    // MongoDB's eget ID for loginoplysningerne
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    // ID på det medlem som loginoplysningerne tilhører
    public int MemberId { get; set; }

    // Medlemmets emailadresse
    public string Email { get; set; } = string.Empty;

    // Gemmer den hash'ede adgangskode
    // Vi gemmer aldrig adgangskoden direkte som almindelig tekst
    public string PasswordHash { get; set; } = string.Empty;
}