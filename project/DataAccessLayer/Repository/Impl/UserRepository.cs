using DataAccessLayer.DataContext;
using DataAccessLayer.Model;

namespace DataAccessLayer.Repository.Impl
{
    public class UserRepository : IEntityRepository<User>
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
            User user = _context.Users.Find(id);

            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public User GetById(int id)
        {
           return _context.Users.Find(id);
        }
    }
}
