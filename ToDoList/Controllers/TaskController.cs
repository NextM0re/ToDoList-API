using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.DataObjects;
using ToDoList.Interfaces;
using ToDoList.Models;
using Task = ToDoList.Models.Task;

namespace ToDoList.Controllers;

[Authorize]
[ApiController]
[Route("/api/tasks")]
public class TaskController(IRepository<Task> taskRepository, IRepository<TaskList> taskListRepisoty) : ControllerBase, ITaskController
{
    // Нет проверки на несуществующий такс лист
    [HttpPost]
    public ActionResult<ResponseTaskDto> CreateTask(RequestTaskDto requestTask)
    {
        var tokenIdResult = ValidateTokenId();
        if (tokenIdResult.Result != null)
            return tokenIdResult.Result;

        if (taskListRepisoty.Find(n => n.Id == requestTask.TaskListId).Count == 0)
            return NotFound("TaskList for this task does not exists!");
        
        var task = new Task(requestTask.Title, requestTask.Description, requestTask.DueDate, DateTime.Now,
            requestTask.Priority, requestTask.TaskListId);
        
        taskRepository.Create(task);

        return Ok(Services.Converter<Task, ResponseTaskDto>.ToDto(task));
    }
    
    [HttpGet]
    public ActionResult<List<ResponseTaskDto>> GetAllTasks()
    {
        var tokenIdResult = ValidateTokenId();
        if (tokenIdResult.Result != null)
            return tokenIdResult.Result;

        var tasks = taskRepository.GetAll();
            
        List<ResponseTaskDto> taskDtos = [];

        foreach (var task in tasks.Where(n => n.TaskList.UserId == int.Parse(tokenIdResult.Value!)))
            taskDtos.Add(Services.Converter<Task, ResponseTaskDto>.ToDto(task));
        
        return Ok(taskDtos);
    }
    
    [HttpGet("{id}")]
    public ActionResult<ResponseTaskDto> GetTask(int id)
    {
        var taskResult = ValidateTask(id);
        if (taskResult.Result != null)
            return taskResult.Result;

        return Ok(Services.Converter<Task, ResponseTaskDto>.ToDto(taskResult.Value!));
    }
    
    [HttpPut("{id}")]
    public ActionResult<ResponseTaskDto> UpdateTask(int id, RequestTaskDto requestTaskDto)
    {
        var taskResult = ValidateTask(id);
        if (taskResult.Result != null)
            return taskResult.Result;
        
        Services.Converter<Task, RequestTaskDto>.UpdateWithDto(taskResult.Value!, requestTaskDto);

        return Ok(Services.Converter<Task, ResponseTaskDto>.ToDto(taskResult.Value!));
    }
    
    [HttpDelete("{id}")]
    public ActionResult<RequestTaskDto> DeleteTask(int id)
    {
        var taskResult = ValidateTask(id);
        if (taskResult.Result != null)
            return taskResult.Result;
        
        taskRepository.Remove(taskResult.Value!);
        
        return Ok(Services.Converter<Task, ResponseTaskDto>.ToDto(taskResult.Value!));
    }
    
    private ActionResult<string> ValidateTokenId()
    {
        var tokenId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
        if (tokenId == null)
            return Unauthorized();
            
        return tokenId;
    }

    private ActionResult<Task> ValidateTask(int id)
    {
        var tokenIdResult = ValidateTokenId();
        if (tokenIdResult.Result != null)
            return tokenIdResult.Result;

        var task = taskRepository.GetById(id);
        if (task == null)
            return NotFound("Task with this id does not exists!");

        if (task.TaskList.UserId != int.Parse(tokenIdResult.Value!))
            return Forbid();

        return task;
    }
}