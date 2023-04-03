using DataAccessLayer.DataContext;
using Task = DataAccessLayer.Model.Task;

namespace DataAccessLayer.Repository.Impl;

public class TaskRepository : IEntityRepository<Task>, ITaskRepository
{
    private readonly TodoListContext _context;

    public TaskRepository(TodoListContext context)
    {
        _context = context;
    }

    public IQueryable<Task> FindAll()
    {
        return _context.Tasks;
    }

    public void Create(Task task)
    {
        _context.Tasks.Add(task);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var task = _context.Tasks.Find(id);

        if (task == null) return;
        _context.Tasks.Remove(task);
        _context.SaveChanges();
    }

    public Task GetById(int id)
    {
        return _context.Tasks.Find(id)!;
    }

    public IQueryable<Task> GetByUserId(int userId)
    {
        return _context.Tasks.Where(t => t.UserId == userId);
    }

    public void Update(Task task)
    {
        _context.Tasks.Update(task);
        _context.SaveChanges();
    }
}