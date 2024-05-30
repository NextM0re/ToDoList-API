using Microsoft.AspNetCore.Mvc;
using ToDoList.DataObjects;

namespace ToDoList.Interfaces;

public interface ITaskController
{
    // Create
    public ActionResult<ResponseTaskDto> CreateTask(RequestTaskDto responseTask);
    
    // Read
    public ActionResult<List<ResponseTaskDto>> GetAllTasks();

    public ActionResult<ResponseTaskDto> GetTask(int id);
    
    // Update
    public ActionResult<ResponseTaskDto> UpdateTask(int id, RequestTaskDto responseTaskDto);
    
    // Delete
    public ActionResult<RequestTaskDto> DeleteTask(int id);

}