namespace ToDoList.DataObjects;

public class ResponseUserDto(int id, string userName, string name)
{
    public int Id { get; set; } = id;

    public string UserName { get; set; } = userName;

    public string Name { get; set; } = name;
}