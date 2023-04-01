using System.Collections.Generic;
using DataAccessLayer.Model;

namespace BusinessLogicLayer.Service;

public interface IUserService
{
    List<User> FindAll();

    User FindById(int id);

    User FindByLogin(string login);

    void RegisterUser(User user);

    bool LoginUser(string login, string password);

    void Delete(int id);
}