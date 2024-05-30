using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Models;

public class TaskList(string title, string? description, int userId)
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [StringLength(64)]
    public string Title { get; set; } = title;
    
    [StringLength(1024)]
    public string? Description { get; set; } = description;
    
    public int UserId { get; set; } = userId;
    
    [ForeignKey("UserId")]
    public virtual User User { get; set; }
    
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}