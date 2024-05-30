using ToDoList.Models;

namespace ToDoList.DataObjects;

public class RequestTaskDto(
    string title,
    string description,
    int taskListId,
    DateTime? dueDate,
    TaskPriority priority)
{
    public string Title { get; set; } = title;

    public string Description { get; set; } = description;
    
    public int TaskListId { get; set; } = taskListId;

    public DateTime? DueDate { get; set; } = dueDate;

    public TaskPriority Priority { get; set; } = priority;

}