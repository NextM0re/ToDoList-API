namespace ToDoList.DataObjects;

public class ResponseTaskListDto(int id, string title, string? description, ICollection<ResponseTaskDto> tasks)
{
     public int Id { get; set; } = id;
     
     public string Title { get; set; } = title;
     
     public string? Description { get; set; } = description;
     
     public virtual ICollection<ResponseTaskDto> Tasks { get; set; } = tasks;

}