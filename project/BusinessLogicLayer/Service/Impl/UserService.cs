using DataAccessLayer.Model;
using DataAccessLayer.Repository;

namespace BusinessLogicLayer.Service.Impl;

public class UserService : IUserService
{
    private readonly IUserRepository _iUserRepository;
    private readonly IEntityRepository<User> _userRepository;

    public UserService(IEntityRepository<User> userRepository, IUserRepository iUserRepository)
    {
        _userRepository = userRepository;
        _iUserRepository = iUserRepository;
    }

    public List<User> FindAll()
    {
        return _userRepository.FindAll().ToList();
    }

    public User FindById(int id)
    {
        return _userRepository.GetById(id);
    }

    public void RegisterUser(User user)
    {
        _userRepository.Create(user);
    }

    public bool LoginUser(string login, string password)
    {
        var user = _userRepository.FindAll().FirstOrDefault(u => u.Login == login);

        if (user == null) return false;


        return password == user.Password;
    }

    public void Delete(int id)
    {
        _userRepository.Delete(id);
    }

    public User FindByLogin(string login)
    {
        return _iUserRepository.GetByLogin(login);
    }
}