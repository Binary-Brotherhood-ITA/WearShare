
using Core;
using MongoDB.Driver;

namespace WS.Backend.Repositories;

public class ClothingRepository
{
    // Gemmer en reference til vores ClothingItems-collection
    private readonly IMongoCollection<ClothingItem> _clothing;

    // Constructor - forbinder repository med MongoDB
    public ClothingRepository(IMongoClient mongoClient, IConfiguration configuration)
    {
        // Henter databasenavnet fra appsettings
        string databaseName = configuration["MongoDb:DatabaseName"] ?? "WearShareDb";

        // Finder vores database
        IMongoDatabase database = mongoClient.GetDatabase(databaseName);

        // Finder vores ClothingItems-collection
        _clothing = database.GetCollection<ClothingItem>("ClothingItems");
    }


    // Henter alt tøj fra MongoDB
    public List<ClothingItem> GetAllClothing()
    {
        // Finder alt tøj og gemmer det i en liste
        List<ClothingItem> clothing = _clothing.Find(item => true).ToList();

        // Returnerer listen med tøj
        return clothing;
    }


    // Henter alt tøj fra et bestemt medlem
    public List<ClothingItem> GetClothingByOwnerId(int ownerId)
    {
        // Finder tøj, hvor OwnerId passer med medlemmets ID
        List<ClothingItem> clothing =
            _clothing.Find(item => item.OwnerId == ownerId).ToList();

        // Returnerer medlemmets tøj
        return clothing;
    }


    // Opretter et nyt stykke tøj i MongoDB
    public void CreateNewClothing(ClothingItem newClothing)
    {
        // Henter alt eksisterende tøj
        List<ClothingItem> clothing = GetAllClothing();

        // Starter med ID 0
        int highestId = 0;

        // Gennemgår alt tøjet
        foreach (ClothingItem item in clothing)
        {
            // Tjekker om tøjets ID er højere
            if (item.Id > highestId)
            {
                // Gemmer det højeste ID
                highestId = item.Id;
            }
        }

        // Giver det nye tøj det næste ID
        newClothing.Id = highestId + 1;

        // Gemmer tøjet i MongoDB
        _clothing.InsertOne(newClothing);
    }


    // Sletter et bestemt stykke tøj ud fra ID
    public bool DeleteClothing(int id)
    {
        // Finder og sletter tøjet med det angivne ID
        DeleteResult result = _clothing.DeleteOne(item => item.Id == id);

        // Returnerer true hvis der blev slettet noget
        return result.DeletedCount > 0;
    }


    // Ændrer om et stykke tøj er tilgængeligt
    public bool ChangeAvailability(int id)
    {
        // Finder tøjet ud fra ID
        ClothingItem? clothing =
            _clothing.Find(item => item.Id == id).FirstOrDefault();

        // Hvis tøjet ikke findes, returnerer vi false
        if (clothing == null)
        {
            return false;
        }

        // Ændrer true til false eller false til true
        clothing.IsAvailable = !clothing.IsAvailable;

        // Gemmer de ændrede oplysninger i MongoDB
        _clothing.ReplaceOne(item => item.Id == id, clothing);

        // Returnerer true når ændringen er gennemført
        return true;
    }


    // Sletter alt tøj, som tilhører et bestemt medlem
    public void DeleteClothingByOwnerId(int ownerId)
    {
        // Sletter alt tøj med medlemmets OwnerId
        _clothing.DeleteMany(item => item.OwnerId == ownerId);
    }
}
