using Repository.Models;

namespace Services.Interface
{
    public interface IUserService
    {
        public User? Get(String Email);
        public void Update(User user);

        public void Delete(User user);

        public List<User> GetAll();

        public void Add(User user);

        public User? checkLogin(string email, string password);

        User? Register(User user);

        void UpdateUser(User user);
    }
}