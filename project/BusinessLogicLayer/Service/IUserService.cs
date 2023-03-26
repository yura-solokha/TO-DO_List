using DataAccessLayer.Model;

namespace BusinessLogicLayer.Service
{
    public interface IUserService
    {
        List<User> FindAll();

        User FindById(int id);

        void RegisterUser(string firstName, string lastName, string login, string password);

        bool LoginUser(string login, string password);

        void Delete(int id);
    }
}
