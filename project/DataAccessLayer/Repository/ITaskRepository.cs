using Task = DataAccessLayer.Model.Task;

namespace DataAccessLayer.Repository;

public interface ITaskRepository
{
    IQueryable<Task> GetByUserId(int userId);

    void Update(Task task);
}