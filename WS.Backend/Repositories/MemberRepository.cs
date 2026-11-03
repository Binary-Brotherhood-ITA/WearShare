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
        // Henter alle medlemmer fra databasen
        List<Member> members = GetAllMembers();

        // Starter med ID 0
        int? highestId = 0;

        // Gennemgår alle medlemmer
        foreach (Member member in members)
        {
            // Tjekker om medlemmets ID er højere
            if (member.Id > highestId)
            {
                // Gemmer det højeste ID
                highestId = member.Id;
            }
        }

        // Giver det nye medlem det næste ID
        newMember.Id = highestId + 1;

        // Gemmer det nye medlem i MongoDB
        _members.InsertOne(newMember);
    }


   
}


