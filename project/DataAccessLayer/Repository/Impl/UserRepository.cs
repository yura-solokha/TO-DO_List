using DataAccessLayer.DataContext;
using DataAccessLayer.Model;

namespace DataAccessLayer.Repository.Impl;

public class UserRepository : IEntityRepository<User>, IUserRepository
{
    private readonly TodoListContext _context;

    public UserRepository(TodoListContext context)
    {
        _context = context;
    }

    public IQueryable<User> FindAll()
    {
        return _context.Users;
    }

    public void Create(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var user = _context.Users.Find(id);

        if (user == null) return;
        _context.Users.Remove(user);
        _context.SaveChanges();
    }

    public User GetById(int id)
    {
        return _context.Users.Find(id)!;
    }

    public User GetByLogin(string login)
    {
        return _context.Users.FirstOrDefault(b => b.Login == login)!;
    }
}