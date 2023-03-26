using DataAccessLayer.DataContext;
using DataAccessLayer.Model;
using DataAccessLayer.Repository;
using DataAccessLayer.Repository.Impl;
using Task = DataAccessLayer.Model.Task;

namespace BusinessLogicLayer.Service.Impl
{
    public class TaskService : ITaskService
    {
        private readonly IEntityRepository<Task> taskRepository;

        public TaskService(IEntityRepository<Task> taskRepository)
        {
            this.taskRepository = taskRepository;
        }

        public void Create(string name, int priority, Category category)
        {
            Task task = new Task();
            task.Name = name;
            task.Priority = priority;
            task.IsDone = false;
            if (category != null)
            {
                task.CategoryId = category.Id;
            }

            taskRepository.Create(task);
        }

        public List<Task> FindAll()
        {
            return taskRepository.FindAll().ToList();
        }

        public Task FindById(int id)
        {
            return taskRepository.GetById(id);
        }

        public void Delete(int id)
        {
            taskRepository.Delete(id);
        }
    }
}
