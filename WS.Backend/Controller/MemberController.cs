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

    // Constructor - modtager vores MemberRepository
    public MemberController(MemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    // GET: api/members
    // Henter alle medlemmer gennem repository
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
    // Finder et bestemt medlem ud fra navnet
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

    
}