using Repository.Models;

namespace Services.Interface
{
    public interface IUserService
    {
        public User? Get(String Email);
        public void Update(User user);

        public List<User> GetAll();

        public User? checkLogin(string email, string password);

        User? Register(User user);

        void UpdateUser(User user);
        User? GetUserWithDonor(int userId);

    }
}