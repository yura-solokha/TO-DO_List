using DataAccessLayer.Model;
using Task = DataAccessLayer.Model.Task;

namespace BusinessLogicLayer.Service
{
    public interface ITaskService
    {
         void Create(string name, int priority, Category category);

         List<Task> FindAll();

         Task FindById(int id);

         void Delete(int id);
    }
}
