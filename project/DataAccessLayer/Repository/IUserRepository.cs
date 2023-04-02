using DataAccessLayer.Model;

namespace DataAccessLayer.Repository;

public interface IUserRepository
{
    User? GetByLogin(string login);
}