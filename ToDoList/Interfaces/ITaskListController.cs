using Microsoft.AspNetCore.Mvc;
using ToDoList.DataObjects;

namespace ToDoList.Interfaces;

public interface ITaskListController
{
    // Create
    public ActionResult<ResponseTaskListDto> CreateTaskList(RequestTaskListDto responseTaskList);
    
    // Read
    public ActionResult<ICollection<ResponseTaskListDto>> GetAllTaskLists();
    
    public ActionResult<ResponseTaskListDto> GetTaskList(int id);

    public ActionResult<ICollection<ResponseTaskDto>> GetTasksFromTaskList(int id);
    
    // Update
    public ActionResult<ResponseTaskListDto> UpdateTaskList(int id, RequestTaskListDto responseTaskList);
    
    // Delete
    public ActionResult<ResponseTaskListDto> DeleteTaskList(int id);

}