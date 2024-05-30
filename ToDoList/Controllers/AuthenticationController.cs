using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ToDoList.DataObjects;
using ToDoList.Interfaces;
using ToDoList.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace ToDoList.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthenticationController(IRepository<User> userRepository, IRepository<TaskList> taskListRepository, IConfiguration configuration) : ControllerBase, IAuthenticationController
{
    private string CreateToken(User user)
    {
        List<Claim> claims = [
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        ];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    [HttpPost("/register")]
    public ActionResult<ResponseUserDto> Register(string userName, string password, string name)
    {
        if (userRepository.Find(n => n.UserName == userName).Count != 0)
            return BadRequest("User with this username already exists");

        var user = new User(userName, BCrypt.Net.BCrypt.HashPassword(password), name);
        
        userRepository.Create(user);
        taskListRepository.Create(new TaskList("default", "default", user.Id));
        
        return Ok(new ResponseUserDto(user.Id, userName, name));
    }
    
    [HttpPost("/login")]
    public IActionResult Login(string userName, string password)
    {
        var user = userRepository.Find(n => n.UserName == userName);
        
        if (user.Count == 0)
            return NotFound();

        if (!BCrypt.Net.BCrypt.Verify(password, user.First().HashPassword))
            return Unauthorized();

        return Ok(CreateToken(user.First()));
    }
    
    [Authorize]
    [HttpPost("/change-password")]
    public IActionResult UpdatePassword(string newPassword)
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (id == null)
            return Unauthorized();
        
        var user = userRepository.GetById(int.Parse(id));

        if (user == null)
            return NotFound();

        user.HashPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
        userRepository.Update(user);

        return Ok();
    }
    
    [Authorize]
    [HttpPost("/change-username")]
    public IActionResult UpdateUserName(string newUserName)
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (id == null)
            return Unauthorized();
        
        var user = userRepository.GetById(int.Parse(id));

        if (user == null)
            return NotFound();
        
        if (userRepository.Find(n => n.UserName == newUserName).Count != 0)
            return BadRequest("User with this username already exists");

        user.UserName = newUserName;
        userRepository.Update(user);

        return Ok();
    }
}