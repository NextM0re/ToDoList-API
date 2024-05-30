using Microsoft.AspNetCore.Mvc;
using ToDoList.DataObjects;

namespace ToDoList.Interfaces;


public interface IUserController
{
    // Read
    public ActionResult<ResponseUserDto> GetUser(int id);
    
    // Update
    public ActionResult<ResponseUserDto> UpdateUser(int id, RequestUserDto newUser);

    // Delete
    public ActionResult<ResponseUserDto> DeleteUser(int id);
}