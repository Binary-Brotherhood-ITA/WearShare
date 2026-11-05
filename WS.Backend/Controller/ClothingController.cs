using Core;
using Microsoft.AspNetCore.Mvc;
using WS.Backend.Repositories;

namespace WS.Backend.Controller;

// Fortæller ASP.NET Core, at klassen er en API-controller.
// [ApiController] håndterer også ugyldige modeldata automatisk med HTTP 400.
[ApiController]
[Route("api/clothing")]
public class ClothingController : ControllerBase
{
    // Giver adgang til metoderne, der arbejder med tøj i MongoDB.
    private readonly ClothingRepository _clothingRepository;

    // Giver adgang til at kontrollere, om et medlem findes i MongoDB.
    private readonly MemberRepository _memberRepository;

    // Constructor: ASP.NET Core giver os de to repositories automatisk.
    public ClothingController(
        ClothingRepository clothingRepository,
        MemberRepository memberRepository)
    {
        _clothingRepository = clothingRepository;
        _memberRepository = memberRepository;
    }

    // GET: api/clothing
    // Henter alt tøj fra databasen.
    [HttpGet]
    public IActionResult GetAllClothing()
    {
        // Henter listen med alle ClothingItem-objekter fra repository.
        List<ClothingItem> clothing = _clothingRepository.GetAllClothing();

        // Returnerer listen til frontend med HTTP 200 OK.
        return Ok(clothing);
    }

    // GET: api/clothing/member/1
    // Henter det tøj, der tilhører et bestemt medlem.
    [HttpGet("member/{ownerId}")]
    public IActionResult GetClothingByOwnerId(int ownerId)
    {
        // Et medlems-ID skal være større end 0.
        if (ownerId <= 0)
        {
            // HTTP 400: Frontend har sendt et ugyldigt ID.
            return BadRequest("Invalid member ID. Please try again.");
        }

        // Finder kun tøj, hvor OwnerId svarer til medlemmets ID.
        List<ClothingItem> clothing =
            _clothingRepository.GetClothingByOwnerId(ownerId);

        // HTTP 200: Returnerer listen, også hvis medlemmet ikke har noget tøj.
        return Ok(clothing);
    }

    // POST: api/clothing
    // Opretter nyt tøj, hvis OwnerId er gyldigt og medlemmet findes.
    [HttpPost]
    public IActionResult CreateNewClothing(ClothingItem newClothing)
    {
        // [ApiController] validerer automatisk felterne i ClothingItem,
        // f.eks. felter markeret med [Required].

        // Kontrollerer, at det valgte medlems-ID er større end 0.
        if (newClothing.OwnerId <= 0)
        {
            // HTTP 400: Tøjet har ikke et gyldigt ejer-ID.
            return BadRequest("Invalid member ID. Please try again.");
        }

        // Kontrollerer gennem MemberRepository, at ejeren findes.
        bool memberExists = _memberRepository.MemberExists(newClothing.OwnerId);

        if (memberExists == false)
        {
            // HTTP 400: Man må ikke oprette tøj med en ejer, der ikke findes.
            return BadRequest("The selected member does not exist.");
        }

        // Gemmer det nye tøj i MongoDB.
        // ClothingRepository tildeler tøjet et ID.
        _clothingRepository.CreateNewClothing(newClothing);

        // HTTP 200: Returnerer det oprettede tøj til frontend.
        return Ok(newClothing);
    }

    // DELETE: api/clothing/1
    // Sletter et bestemt stykke tøj ud fra tøjets ID.
    [HttpDelete("{id}")]
    public IActionResult DeleteClothing(int id)
    {
        // Kontrollerer, at tøjets ID er større end 0.
        if (id <= 0)
        {
            // HTTP 400: ID'et er ugyldigt.
            return BadRequest("Invalid clothing item. Please try again.");
        }

        // Forsøger at slette tøjet i MongoDB.
        bool deleted = _clothingRepository.DeleteClothing(id);

        if (deleted == false)
        {
            // HTTP 404: ID'et er gyldigt, men tøjet findes ikke.
            return NotFound("Clothing item not found.");
        }

        // HTTP 200: Tøjet blev slettet.
        return Ok("Clothing item deleted.");
    }

    // PATCH: api/clothing/1/availability
    // Skifter IsAvailable mellem true og false for et bestemt stykke tøj.
    [HttpPatch("{id}/availability")]
    public IActionResult ChangeAvailability(int id)
    {
        // Kontrollerer, at tøjets ID er større end 0.
        if (id <= 0)
        {
            // HTTP 400: ID'et er ugyldigt.
            return BadRequest("Invalid clothing item. Please try again.");
        }

        // ClothingRepository finder tøjet og ændrer dets tilgængelighed.
        bool changed = _clothingRepository.ChangeAvailability(id);

        if (changed == false)
        {
            // HTTP 404: Tøjet findes ikke i MongoDB.
            return NotFound("Clothing item not found.");
        }

        // HTTP 200: Tøjets tilgængelighed blev ændret.
        return Ok("Clothing availability changed.");
    }

    // Skifter IsAvailable mellem true og false og indsætter BorrowerId
    // [FromQuery] fortæller controlleren, at den skal hente userId fra URL'ens query string.
    [HttpPut("{id}/borrowrent")]
    public IActionResult BorrowOrReturn(int id, [FromQuery] int? userId)
    {
        if (id <= 0)
        {
            // HTTP 400: ID'et er ugyldigt.
            return BadRequest("Invalid clothing item. Please try again.");
        }
        // Vi skal også kunne sende userId null, når brugeren afleverer tøjet. 
        if (userId <= 0)
        {
            return BadRequest("Invalid user ID. Please try again.");
        }
        bool isSuccess = _clothingRepository.BorrowOrReturn(id, userId);
        if(!isSuccess)
        {
            return Conflict("Clothing item could not be borrowed or returned.");
        }
        // HTTP 200: Tøjets tilgængelighed blev ændret.
        return Ok("Clothing availability changed.");
    }
}
