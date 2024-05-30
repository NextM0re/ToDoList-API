using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Models;

public class Task
{
    
    public Task(string title, string description, DateTime? dueDate, DateTime creationDate, TaskPriority priority, int taskListId)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        CreationDate = creationDate;
        Priority = priority;
        TaskListId = taskListId;
    }
    
    // public Task() { }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [StringLength(64)] 
    public string Title { get; set; }
    
    [StringLength(1024)]
    public string Description { get; set; }

    public DateTime? DueDate { get; set; }
    
    public DateTime CreationDate { get; set; }
    
    public TaskPriority Priority { get; set; }
    
    public int TaskListId { get; set; }
    
    [ForeignKey("TaskListId")]
    public virtual TaskList TaskList { get; set; }
}