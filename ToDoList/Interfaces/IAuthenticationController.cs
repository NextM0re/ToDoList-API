using Microsoft.AspNetCore.Mvc;
using ToDoList.DataObjects;

namespace ToDoList.Interfaces;

public interface IAuthenticationController
{
    // Auth
    public ActionResult<ResponseUserDto> Register(string userName, string password, string name);

    public IActionResult Login(string userName, string password);
    
    // Update
    public IActionResult UpdatePassword(string newPassword);
    
    public IActionResult UpdateUserName(string newUserName);

}