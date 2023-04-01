using DataAccessLayer.Repository;
using Task = DataAccessLayer.Model.Task;

namespace BusinessLogicLayer.Service.Impl;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _iTaskRepository;
    private readonly IEntityRepository<Task> _taskRepository;

    public TaskService(IEntityRepository<Task> taskRepository, ITaskRepository iTaskRepository)
    {
        _taskRepository = taskRepository;
        _iTaskRepository = iTaskRepository;
    }

    public void Create(Task task)
    {
        _taskRepository.Create(task);
    }

    public List<Task> FindAll()
    {
        return _taskRepository.FindAll().ToList();
    }

    public Task FindById(int id)
    {
        return _taskRepository.GetById(id);
    }

    public void Delete(int id)
    {
        _taskRepository.Delete(id);
    }

    public List<Task> FindForUser(int userId)
    {
        return _iTaskRepository.GetByUserId(userId).ToList();
    }
}