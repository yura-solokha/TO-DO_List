using DataAccessLayer.DataContext;
using Microsoft.EntityFrameworkCore;
using Task = DataAccessLayer.Model.Task;

namespace DataAccessLayer.Repository.Impl
{
    public class TaskRepository : IEntityRepository<Task>
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
            Task task = _context.Tasks.Find(id);

            if (task != null)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }
        }


        public Task GetById(int id)
        {
            return _context.Tasks.Find(id);
        }

    }
}
