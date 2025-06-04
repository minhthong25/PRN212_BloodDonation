using Repository.Interface;
using Repository.Models;
using Repository.Repository;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;

        public UserService()
        {
            _userRepository = new GenericRepository<User>();
        }

        public void Add(User user)
        {
            _userRepository.Add(user);
        }

        public User? checkLogin(string email, string password)
        {
            User? user = _userRepository.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                if (user.Password == password)
                {
                    return user;
                }
            }
            return null;
        }

        public void Delete(User user)
        {
            _userRepository.Delete(user);
        }

        public User? Get(string Email)
        {
            return _userRepository.FirstOrDefault(u => u.Email == Email);
        }    
        
        public List<User> GetAll()
        {
            return _userRepository.GetAll().ToList();
        }

        public void Update(User user)
        {
            _userRepository.Update(user);
        }
    }
}
