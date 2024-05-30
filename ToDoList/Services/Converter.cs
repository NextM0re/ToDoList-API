using System.Runtime.InteropServices.ComTypes;
using ToDoList.DataObjects;
using ToDoList.Models;
using Task = ToDoList.Models.Task;

namespace ToDoList.Services;

using System;
using System.Reflection;

public abstract class Converter<T, U> where T: class where U: class 
{
    public static void UpdateWithDto(T entity, U entityDto)
    {
        var entityType = typeof(T);
        var entityDtoType = typeof(U);

        foreach (var entityDtoProp in entityDtoType.GetProperties())
        {
            var entityProp = entityType.GetProperty(entityDtoProp.Name);
            if (entityProp != null && entityProp.CanWrite)
            {
                var value = entityDtoProp.GetValue(entityDto);
                entityProp.SetValue(entity, value);
            }
        }
    }
    
    public static U ToDto(T entity)
    {
        var entityType = typeof(T);
        var entityDtoType = typeof(U);
        
        var constructor = entityDtoType.GetConstructors().FirstOrDefault();
        
        if (constructor == null)
            throw new InvalidOperationException("DTO class must have a constructor");

        var parameters = constructor.GetParameters();
        var args = new object[parameters.Length];
        
        for (int i = 0; i < parameters.Length; i++)
        {
            var param = parameters[i];
            
            var entityProp = entityType.GetProperty(param.Name!, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            
            if (entityProp == null)
                continue;
            
            if (param.ParameterType == typeof(ICollection<ResponseTaskDto>))
            {
                var tasks = entityProp.GetValue(entity)!;
                
                if (tasks is ICollection<Task> taskCollection)
                {
                    var dtos = new List<ResponseTaskDto>();
                    
                    foreach (var task in taskCollection)
                    {
                        dtos.Add(Converter<Task, ResponseTaskDto>.ToDto(task));
                    }
                    
                    args[i] = dtos;
                    
                    continue;
                }
                
            }
            args[i] = entityProp.GetValue(entity)!;
        }
        
        var dto = (U)constructor.Invoke(args);

        return dto;
    }
}
