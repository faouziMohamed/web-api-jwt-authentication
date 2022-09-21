using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using authTests.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace authTests.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
  private readonly IConfiguration _config;
  public LoginController(IConfiguration config)
  {
    _config = config;

  }
  [AllowAnonymous]
  [HttpPost]
  public IActionResult Login([FromBody] UserLogin userLogin)
  {
    try
    {
      var user = AuthenticateUser(userLogin);
      if (user == null) return NotFound("User not found");
      string tokenString = GenerateJsonWebToken(user);

      return Ok(tokenString);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  private string GenerateJsonWebToken(UserModel user)
  {
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
      new Claim(ClaimTypes.NameIdentifier, user.Username),
      new Claim(ClaimTypes.Email, user.EmailAddress),
      new Claim(ClaimTypes.GivenName, user.GivenName),
      new Claim(ClaimTypes.Surname, user.Surname),
      new Claim(ClaimTypes.Role, user.Role)
    };

    var token = new JwtSecurityToken(
      issuer: _config["Jwt:Issuer"],
      audience: _config["Jwt:Issuer"],
      claims,
      expires: DateTime.Now.AddMinutes(120),
      signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }
  private static UserModel? AuthenticateUser(UserLogin userLogin)
  {
    string username = userLogin.Username.ToLower();
    return UserConstants
      .Users
      .FirstOrDefault(u =>
        u.Username == username && u.Password == userLogin.Password
      );
  }
}
