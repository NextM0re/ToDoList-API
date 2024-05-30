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
[Route("/api/tasklists")]
public class TaskListController(IRepository<TaskList> repository) : ControllerBase, ITaskListController
{
        
    [HttpPost]
    [Route("/create")]
    public ActionResult<ResponseTaskListDto> CreateTaskList(RequestTaskListDto requestTaskListTaskList)
    {
        var tokenIdResult = ValidateTokenId();
        if (tokenIdResult.Result != null)
            return tokenIdResult.Result;

        var taskList = new TaskList(requestTaskListTaskList.Title, requestTaskListTaskList.Description,
            int.Parse(tokenIdResult.Value!));

        repository.Create(taskList);

        return Ok(Services.Converter<TaskList, ResponseTaskListDto>.ToDto(taskList));
    }

    [HttpGet]
    [Route("/getall")]
    public ActionResult<ICollection<ResponseTaskListDto>> GetAllTaskLists()
    {
        var tokenIdResult = ValidateTokenId();
        if (tokenIdResult.Result != null)
            return tokenIdResult.Result;

        var taskLists = repository.GetAll();
            
        List<ResponseTaskListDto> taskListDtos = [];

        foreach (var taskList in taskLists.Where(n => n.UserId == int.Parse(tokenIdResult.Value!)))
            taskListDtos.Add(Services.Converter<TaskList, ResponseTaskListDto>.ToDto(taskList));
        
        return Ok(taskListDtos);
    }

    [HttpGet]
    [Route("getbyid")]
    public ActionResult<ResponseTaskListDto> GetTaskList(int id)
    {
        var taskListResult = ValidateTaskList(id);
        if (taskListResult.Result != null)
            return taskListResult.Result;

        return Ok(Services.Converter<TaskList, ResponseTaskListDto>.ToDto(taskListResult.Value!));
    }

    [HttpGet]
    [Route("/gettasks")]
    public ActionResult<ICollection<ResponseTaskDto>> GetTasksFromTaskList(int taskListId)
    {
        var taskListResult = ValidateTaskList(taskListId);
        if (taskListResult.Result != null)
            return taskListResult.Result;

        List<ResponseTaskDto> taskDtos = [];
        
        foreach (var task in taskListResult.Value!.Tasks)
            taskDtos.Add(Services.Converter<Task, ResponseTaskDto>.ToDto(task));
        
        return Ok(taskDtos);
    }

    [HttpPut]
    [Route("/update")]
    public ActionResult<ResponseTaskListDto> UpdateTaskList(int id, RequestTaskListDto requestTaskList)
    {
        var taskListResult = ValidateTaskList(id);
        if (taskListResult.Result != null)
            return taskListResult.Result;

        Services.Converter<TaskList, RequestTaskListDto>.UpdateWithDto(taskListResult.Value!, requestTaskList);

        return Ok(Services.Converter<TaskList, ResponseTaskListDto>.ToDto(taskListResult.Value!));
    }

    [HttpDelete("/delete")]
    public ActionResult<ResponseTaskListDto> DeleteTaskList(int id)
    {
        var taskListResult = ValidateTaskList(id);
        if (taskListResult.Result != null)
            return taskListResult.Result;

        repository.Remove(taskListResult.Value!);

        return Ok(Services.Converter<TaskList, ResponseTaskListDto>.ToDto(taskListResult.Value!));
    }
        
    private ActionResult<string> ValidateTokenId()
    {
        var tokenId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
        if (tokenId == null)
            return Unauthorized();
            
        return tokenId;
    }

    private ActionResult<TaskList> ValidateTaskList(int id)
    {
        var tokenIdResult = ValidateTokenId();
        if (tokenIdResult.Result != null)
            return tokenIdResult.Result;

        var taskList = repository.GetById(id);
        if (taskList == null)
            return NotFound("Task list with this id does not exists!");

        if (taskList.UserId != int.Parse(tokenIdResult.Value!))
            return Forbid();

        return taskList;
    }

}