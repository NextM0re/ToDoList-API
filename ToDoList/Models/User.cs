using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Models;

public class User (string userName, string hashPassword, string name)
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [StringLength(64)]
    public string UserName { get; set; } = userName;
    
    [StringLength(256)]
    public string HashPassword { get; set; } = hashPassword;
    
    [StringLength(16)]
    public string Name { get; set; } = name;
    
    public virtual ICollection<TaskList> TaskLists { get; set; } = new List<TaskList>();
}