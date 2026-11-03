using Core;
using MongoDB.Driver;

namespace WS.Backend.Repositories;

public class MemberRepository
{

// Gemmer en reference til vores Members-collection i MongoDB
    private readonly IMongoCollection<Member> _members;

// Constructor - kaldes, når MemberRepository bliver oprettet
    public MemberRepository(IMongoClient mongoClient, IConfiguration configuration)
    {
        // Henter databasenavnet fra appsettings.Development.json
        string databaseName = configuration["MongoDb:DatabaseName"] ?? "WearShareDb";

        // Finder vores database i MongoDB Atlas
        IMongoDatabase database = mongoClient.GetDatabase(databaseName);

        // Finder Members-collection i databasen
        _members = database.GetCollection<Member>("Members");
    }
    
    // Henter alle medlemmer fra MongoDB
    public List<Member> GetAllMembers()
    {
        // Finder alle medlemmer i vores Members-collection
        List<Member> members = _members.Find(member => true).ToList();

        // Returnerer listen med medlemmer
        return members;
    }
    
    // Opretter et nyt medlem i MongoDB
    public void CreateNewMember(Member newMember)
    {
        // Gemmer det nye medlem i vores Members-collection
        _members.InsertOne(newMember);
    }
    
}



