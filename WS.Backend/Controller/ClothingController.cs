
using Core;
using Microsoft.AspNetCore.Mvc;
using WS.Backend.Repositories;

namespace WS.Backend.Controller;

[ApiController]
[Route("api/clothing")]
public class ClothingController : ControllerBase
{
    // Gemmer en reference til vores ClothingRepository
    private readonly ClothingRepository _clothingRepository;

    // Constructor - modtager vores ClothingRepository
    public ClothingController(ClothingRepository clothingRepository)
    {
        _clothingRepository = clothingRepository;
    }


    // GET: api/clothing
    // Henter alt tøj fra databasen
    [HttpGet]
    public IActionResult GetAllClothing()
    {
        // Kalder metoden i repository
        List<ClothingItem> clothing = _clothingRepository.GetAllClothing();

        // Returnerer tøjet med HTTP-status 200
        return Ok(clothing);
    }


    // GET: api/clothing/member/1
    // Henter tøj fra et bestemt medlem
    [HttpGet("member/{ownerId}")]
    public IActionResult GetClothingByOwnerId(int ownerId)
    {
        // Kontrollerer at ejerens ID er gyldigt
        if (ownerId <= 0)
        {
            return BadRequest("Owner ID must be greater than 0");
        }

        // Henter medlemmets tøj gennem repository
        List<ClothingItem> clothing =
            _clothingRepository.GetClothingByOwnerId(ownerId);

        // Returnerer listen med tøj
        return Ok(clothing);
    }


    // POST: api/clothing
    // Opretter et nyt stykke tøj
    [HttpPost]
    public IActionResult CreateNewClothing(ClothingItem newClothing)
    {
        // Kontrollerer at tøjet har et gyldigt ejer-ID
        if (newClothing.OwnerId <= 0)
        {
            return BadRequest("Owner ID must be greater than 0");
        }

        // Kalder repository for at gemme tøjet
        _clothingRepository.CreateNewClothing(newClothing);

        // Returnerer HTTP 200 og det oprettede tøj
        return Ok(newClothing);
    }


    // DELETE: api/clothing/1
    // Sletter et bestemt stykke tøj
    [HttpDelete("{id}")]
    public IActionResult DeleteClothing(int id)
    {
        // Kalder repository for at slette tøjet
        bool deleted = _clothingRepository.DeleteClothing(id);

        // Hvis tøjet ikke blev fundet, returnerer vi 404
        if (deleted == false)
        {
            return NotFound("Clothing item not found");
        }

        // Returnerer HTTP 200 når tøjet er slettet
        return Ok("Clothing item deleted");
    }


    // PATCH: api/clothing/1/availability
    // Ændrer tøjets tilgængelighed
    [HttpPatch("{id}/availability")]
    public IActionResult ChangeAvailability(int id)
    {
        // Kalder repository for at ændre tilgængeligheden
        bool changed = _clothingRepository.ChangeAvailability(id);

        // Hvis tøjet ikke findes, returnerer vi 404
        if (changed == false)
        {
            return NotFound("Clothing item not found");
        }

        // Returnerer HTTP 200 når tøjet er ændret
        return Ok("Clothing availability changed");
    }
}
