using DataAccessLayer.Repository;
using Microsoft.Extensions.Logging;
using Task = DataAccessLayer.Model.Task;

namespace BusinessLogicLayer.Service.Impl;

public class TaskService : ITaskService
{
    private readonly ILogger<TaskService> _logger;
    private readonly ITaskRepository _iTaskRepository;
    private readonly IEntityRepository<Task> _taskRepository;

    public TaskService(IEntityRepository<Task> taskRepository, ITaskRepository iTaskRepository,
        ILogger<TaskService> logger)
    {
        _taskRepository = taskRepository;
        _iTaskRepository = iTaskRepository;
        _logger = logger;
    }

    public void Create(Task task)
    {
        _logger.LogInformation("Created task {}.", task);
        _taskRepository.Create(task);
    }

    public void Update(Task task)
    {
        _logger.LogInformation("Updated task {}.", task);
        _iTaskRepository.Update(task);
    }

    public List<Task> FindAll()
    {
        _logger.LogInformation("Found all tasks.");
        return _taskRepository.FindAll().ToList();
    }

    public Task FindById(int id)
    {
        _logger.LogInformation("Get task by id={}.", id);
        return _taskRepository.GetById(id);
    }

    public void Delete(int id)
    {
        _logger.LogInformation("Delete task with id={}.", id);
        _taskRepository.Delete(id);
    }

    public List<Task> FindForUser(int userId)
    {
        _logger.LogInformation("Get tasks for user with id={}.", userId);
        return _iTaskRepository.GetByUserId(userId).ToList();
    }

    public List<Task> FilterTasks(List<Task> tasks, int? categoryId, bool? isDone, int? priority)
    {
        switch (true)
        {
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