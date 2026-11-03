using Core;
using MongoDB.Driver;

namespace WS.Backend.Repositories;

public class MemberRepository
{

// Gemmer en reference til vores Members-collection i MongoDB
    private readonly IMongoCollection<Member> _members;

// Gemmer en reference til ClothingRepository
    private readonly ClothingRepository _clothingRepository;



// Constructor - modtager MongoDB og ClothingRepository
    public MemberRepository(
        IMongoClient mongoClient,
        IConfiguration configuration,
        ClothingRepository clothingRepository)
    {
        // Henter databasenavnet fra appsettings
        string databaseName =
            configuration["MongoDb:DatabaseName"] ?? "WearShareDb";

        // Finder vores database i MongoDB Atlas
        IMongoDatabase database = mongoClient.GetDatabase(databaseName);

        // Finder Members-collection
        _members = database.GetCollection<Member>("Members");

        // Gemmer vores ClothingRepository
        _clothingRepository = clothingRepository;
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

    
    // Finder et medlem i databasen ud fra navnet
    public Member? FindOneMember(string memberName)
    {
        // Søger efter et medlem med det angivne navn
        // FirstOrDefault(): Returner først fundne resultat ellers returner null
        // _members.Find() = Søger i vores MongoDB-collection
        Member? member = _members.Find(member => member.Name == memberName).FirstOrDefault();

        // Returnerer medlemmet
        return member;
    }

    
    // Sletter et medlem og alt tøj, som medlemmet ejer
    public bool DeleteMemberById(int id)
    {
        // Finder og sletter medlemmet med det angivne ID
        DeleteResult result = _members.DeleteOne(member => member.Id == id);

        // Tjekker om der faktisk blev slettet et medlem
        if (result.DeletedCount == 0)
        {
            // Returnerer false, hvis medlemmet ikke findes
            return false;
        }

        // Sletter alt tøj, hvor OwnerId er lig med medlemmets ID
        _clothingRepository.DeleteClothingByOwnerId(id);

        // Returnerer true, når sletningen er gennemført
        return true;
    }

    
    // Tjekker om et medlem findes ud fra dets ID
    public bool MemberExists(int id)
    {
        // Søger efter et medlem med det angivne ID
        Member? member = _members.Find(m => m.Id == id).FirstOrDefault();

        // Hvis member ikke er null, findes medlemmet i databasen
        return member != null;
    }


   
}


