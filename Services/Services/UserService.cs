using Repository.Models;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class UserService
    {
        private static readonly UserRepository _userRepository = new UserRepository();
        public User? checkLogin(string email, string password)
        {
            User user = _userRepository.Get(email);
            if (user != null)
            {
                if (user.Password == password)
                {
                    return user;
                }
            }
            return null;
        }

        public void Update(User user)
        {
            if (user != null)
            {
                _userRepository.Update(user);
            }
        }

        public void AddUser(User user)
        {
            if (user != null)
            {
                _userRepository.Add(user);
            }
        }
    }
}
