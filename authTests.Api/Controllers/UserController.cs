using System.Security.Claims;
using authTests.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authTests.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
  [HttpGet("public")]
  public IActionResult Public()
  {
    var user = GetCurrentUser();

    return Ok($"This is a public endpoint: {user}");
  }

  [HttpGet("Admins")]
  [Authorize(Roles = "Admin")]
  public IActionResult AdminsEndpoints()
  {
    var user = GetCurrentUser();
    return Ok($"Hi {user.GivenName}, you are a {user.Role} and you can access this endpoint");
  }

  [HttpGet("users")]
  [Authorize(Roles = "User")]
  public IActionResult UsersEndpoints()
  {
    var user = GetCurrentUser();
    return Ok($"Hi {user?.GivenName}, you are a {user?.Role} and you can access this endpoint");
  }

  [HttpGet("manage")]
  [Authorize(Roles = "Admin,User")]
  public IActionResult AdminsUsersEndpoints()
  {
    var user = GetCurrentUser();
    return Ok($"Hi {user?.GivenName}, you are a {user?.Role} and you can access this endpoint");
  }

  private UserModel? GetCurrentUser()
  {

    var identity = HttpContext.User.Identity as ClaimsIdentity;

    if (identity == null) return null;
    IEnumerable<Claim> claims = identity.Claims;
    return new UserModel
    {
      Username = claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
      EmailAddress = claims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
      Role = claims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value,
      GivenName = claims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
      Surname = claims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value
    };
  }
}
