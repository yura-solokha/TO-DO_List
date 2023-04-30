using System.Collections.Generic;
using Task = DataAccessLayer.Model.Task;

namespace BusinessLogicLayer.Service;

public interface ITaskService
{
    void Create(Task task);

    void Update(Task task);

    List<Task> FindAll();

    Task FindById(int id);

    void Delete(int id);

    List<Task> FindForUser(int userId);

    List<Task> FilterTasks(List<Task> tasks, int? categoryId, bool? isDone, int? priority);
    
    void MarkSubtasks(Task task);
}