using DataAccessLayer.Repository;
using Microsoft.Extensions.Logging;
using Task = DataAccessLayer.Model.Task;

namespace BusinessLogicLayer.Service.Impl;

public class TaskService : ITaskService
{
    private readonly ILogger<TaskService> _logger;
    private readonly ITaskRepository _taskRepository;
    private readonly IEntityRepository<Task> _taskEntityRepository;

    public TaskService(IEntityRepository<Task> taskEntityRepository, ITaskRepository taskRepository,
        ILogger<TaskService> logger)
    {
        _taskEntityRepository = taskEntityRepository;
        _taskRepository = taskRepository;
        _logger = logger;
    }

    public void Create(Task task)
    {
        _logger.LogInformation("Created task {}.", task);
        _taskEntityRepository.Create(task);
    }

    public void Update(Task task)
    {
        _logger.LogInformation("Updated task {}.", task);
        _taskRepository.Update(task);
    }

    public List<Task> FindAll()
    {
        _logger.LogInformation("Found all tasks.");
        return _taskEntityRepository.FindAll().ToList();
    }

    public Task FindById(int id)
    {
        _logger.LogInformation("Get task by id={}.", id);
        return _taskEntityRepository.GetById(id);
    }

    public void Delete(int id)
    {
        _logger.LogInformation("Delete task with id={}.", id);
        _taskEntityRepository.Delete(id);
    }

    public List<Task> FindForUser(int userId)
    {
        _logger.LogInformation("Get tasks for user with id={}.", userId);
        return _taskRepository.GetByUserId(userId).ToList();
    }

    public List<Task> FilterTasks(List<Task> tasks, int? categoryId, bool? isDone, int? priority)
    {
        switch (true)
        {
            case var _ when categoryId != null && isDone != null && priority != null:
                _logger.LogInformation("Filter tasks by category, isDone, and priority.");
                tasks = tasks.Where(t => t.CategoryId == categoryId && t.IsDone == isDone && t.Priority == priority)
                    .ToList();
                break;
            case var _ when categoryId != null && isDone != null:
                _logger.LogInformation("Filter tasks by category and isDone.");
                tasks = tasks.Where(t => t.CategoryId == categoryId && t.IsDone == isDone).ToList();
                break;
            case var _ when categoryId != null && priority != null:
                _logger.LogInformation("Filter tasks by category and priority.");
                tasks = tasks.Where(t => t.CategoryId == categoryId && t.Priority == priority).ToList();
                break;
            case var _ when isDone != null && priority != null:
                _logger.LogInformation("Filter tasks by isDone and priority.");
                tasks = tasks.Where(t => t.IsDone == isDone && t.Priority == priority).ToList();
                break;
            case var _ when categoryId != null:
                _logger.LogInformation("Filter tasks by category.");
                tasks = tasks.Where(t => t.CategoryId == categoryId).ToList();
                break;
            case var _ when isDone != null:
                _logger.LogInformation("Filter tasks by isDone field.");
                tasks = tasks.Where(t => t.IsDone == isDone).ToList();
                break;
            case var _ when priority != null:
                _logger.LogInformation("Filter tasks by priority.");
                tasks = tasks.Where(t => t.Priority == priority).ToList();
                break;
        }

        return tasks;
    }
}