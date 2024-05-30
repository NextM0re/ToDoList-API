using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.DataObjects;
using ToDoList.Interfaces;
using ToDoList.Models;

namespace ToDoList.Controllers;

[Authorize]
[ApiController]
[Route("/api/users")]
public class UserController (IRepository<User> repository): ControllerBase, IUserController
{
    [HttpGet("/{id}")]
    public ActionResult<ResponseUserDto> GetUser(int id)
    {
        var result = ValidateUser(id);
        var user = result.Value;
        
        if (user == null)
            return result.Result!;
        
        return Ok(Services.Converter<User, ResponseUserDto>.ToDto(user));
    }
    
    [HttpPut("/{id}")]
    public ActionResult<ResponseUserDto> UpdateUser(int id, RequestUserDto newUser)
    {
        var result = ValidateUser(id);
        var user = result.Value;
        
        if (user == null)
            return result.Result!;

        Services.Converter<User, RequestUserDto>.UpdateWithDto(user, newUser);
        
        repository.Update(user);

        return Ok(Services.Converter<User, ResponseUserDto>.ToDto(user));

    }
    
    [HttpDelete("/{id}")]
    public ActionResult<ResponseUserDto> DeleteUser(int id)
    {
        var result = ValidateUser(id);
        var user = result.Value;
        
        if (user == null)
            return result.Result!;
        
        repository.Remove(user);

        return Ok(Services.Converter<User, ResponseUserDto>.ToDto(user));
    }
    
    private ActionResult<string> ValidateTokenId()
    {
        var tokenId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
        if (tokenId == null)
            return Unauthorized();
            
        return tokenId;
    }
    
    private ActionResult<User> ValidateUser(int id = -1)
    {
        var tokenIdResult = ValidateTokenId();
        if (tokenIdResult.Result != null)
            return tokenIdResult.Result;
    
        var userId = int.Parse(tokenIdResult.Value!);
        var user = repository.GetById(userId);
    
        if (user == null)
            return NotFound("User with this id does not exists");
    
        if (id > -1 && userId != id)
            return Unauthorized();
    
        return user;
    }
}