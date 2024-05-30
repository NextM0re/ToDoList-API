using ToDoList.Models;

namespace ToDoList.DataObjects;

public class ResponseTaskDto(
    int id,
    string title,
    string description,
    DateTime? dueDate,
    DateTime creationDate,
    TaskPriority priority,
    int taskListId)
{
    public int Id { get; set; } = id;

    public string Title { get; set; } = title;

    public string Description { get; set; } = description;

    public DateTime? DueDate { get; set; } = dueDate;

    public DateTime CreationDate { get; set; } = creationDate;

    public TaskPriority Priority { get; set; } = priority;

    public int TaskListId { get; set; } = taskListId;
}