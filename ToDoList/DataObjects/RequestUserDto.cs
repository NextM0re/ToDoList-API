namespace ToDoList.DataObjects;

public class RequestUserDto(string name)
{
    public string Name { get; set; } = name;
}