using DataAccessLayer.Model;
using DataAccessLayer.Repository;

namespace BusinessLogicLayer.Service.Impl
{
    public class UserService : IUserService
    {
        private readonly IEntityRepository<User> userRepository;


        public UserService(IEntityRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }

        public List<User> FindAll()
        {
            return userRepository.FindAll().ToList();
        }

        public User FindById(int id)
        {
            return userRepository.GetById(id);
        }

        public void RegisterUser(string firstName, string lastName, string login, string password)
        {
            var user = new User
            {
                LastName = lastName,
                FirstName = firstName,
                Login = login,
                Password = password
            };

            userRepository.Create(user);
        }

        public bool LoginUser(string login, string password)
        {

            var user = userRepository.FindAll().FirstOrDefault(u => u.Login == login);

            if (user == null)
            {
                return false;
            }


            return password == user.Password;
        }

        public void Delete(int id)
        {
            userRepository.Delete(id);
        }
    }
}
