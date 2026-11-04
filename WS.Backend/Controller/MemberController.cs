using Core;
using Microsoft.AspNetCore.Mvc;
using WS.Backend.Repositories;

namespace WS.Backend.Controller;

//Forklaring:
//"[ApiController]": Fortæller, at klassen er en API-controller
// "[Route("api/members")]": Bestemmer API'ets adresse
// "[HttpGet]": Angiver, at metoden håndterer GET-kald
// "MemberRepository": Giver adgang til vores repository
// "GetAllMembers()": Henter alle medlemmer
// "Ok(members)" Returnerer HTTP 200 og medlemslisten

[ApiController]
[Route("api/members")]
public class MemberController : ControllerBase
{
// Gemmer en reference til vores repository
    private readonly MemberRepository _memberRepository;

// Gemmer en reference til vores ClothingRepository
    private readonly ClothingRepository _clothingRepository;
    
// Constructor - modtager begge repositories
    public MemberController( MemberRepository memberRepository, ClothingRepository clothingRepository)
    {
        // Gemmer vores MemberRepository
        _memberRepository = memberRepository;
        
        // Gemmer vores ClothingRepository
        _clothingRepository = clothingRepository;
    }

    // GET: api/members
    // Henter alle medlemmer gennem repository
    // Ok(members) pakker listen ind i HTTP-svar med statuskode 200 OK
    // IActionResult kan returnere forskellige HTTP-svar (fx NotFound() 404, BadRequest() 400, NoContent() 204...)
    [HttpGet]
    public IActionResult GetAllMembers()
    {
        // Kalder metoden i vores repository
        List<Member> members = _memberRepository.GetAllMembers();

        // Returnerer medlemmerne med HTTP-status 200
        return Ok(members);
    }
    
    // POST: api/members
    // Opretter et nyt medlem gennem repository
    [HttpPost]
    public IActionResult CreateNewMember(Member newMember)
    {
        // Kalder repository for at gemme medlemmet
        _memberRepository.CreateNewMember(newMember);

        // Returnerer HTTP 200 og det oprettede medlem
        return Ok(newMember);
    }
    
    
    // GET: api/members/Amina
    // Finder et member ud fra navnet
    [HttpGet("{memberName}")]
    public IActionResult FindOneMember(string memberName)
    {
        // Kalder repository for at finde medlemmet
        Member? member = _memberRepository.FindOneMember(memberName);

        // Hvis medlemmet ikke findes, returneres 404
        if (member == null)
        {
            return NotFound("Medlemmet blev ikke fundet");
        }

        // Returnerer medlemmet med HTTP-status 200
        return Ok(member);
    }

    
    // DELETE: api/members/1
// Sletter et medlem og medlemmets tøj
    [HttpDelete("{id:int}")]
    public IActionResult DeleteMemberById(int id)
    {
        // Kontrollerer at ID er større end 0
        if (id <= 0)
        {
            return BadRequest("Member ID must be greater than 0");
        }

        // Kalder repository for at slette medlemmet
        bool deleted = _memberRepository.DeleteMemberById(id);

        // Tjekker om medlemmet blev slettet
        if (deleted == false)
        {
            // Returnerer HTTP 404 hvis medlemmet ikke blev fundet
            return NotFound("Member not found");
        }

        // Returnerer HTTP 200 når medlemmet er slettet
        return Ok("Member and clothing deleted");
    }

    
}