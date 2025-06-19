using Microsoft.EntityFrameworkCore;
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
        private readonly BloodDonationDbContext _context;

        public UserService()
        {
            _userRepository = new GenericRepository<User>();
            _context = new BloodDonationDbContext();
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
        public User? checkLogin(string email, string password)
        {
            var user = _userRepository.GetAll().FirstOrDefault(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
                u.Password.Equals(password));
            return user;
        }

        public User? Register(User user)
        {
            // Check if email already exists
            if (_userRepository.GetAll().Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
            {
                return null;
            }
            // Set default values
            user.CreatedAt = DateTime.Now;
            user.IsActive = true;
            user.Role = "User"; // Default role

            _userRepository.Add(user);
            _userRepository.Save();
            return user;
        }

        public void UpdateUser(User user)
        {
            var existingUser = _userRepository.GetAll().FirstOrDefault(u => u.UserId == user.UserId);
            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            // Update only allowed fields
            existingUser.FullName = user.FullName;
            existingUser.Email = user.Email;
            existingUser.Phone = user.Phone;

            _userRepository.Update(existingUser);
            _userRepository.Save();
        }

        public User? GetUserWithDonor(int userId)
        {
            return _context.Users
                .Include(u => u.Donor)
                    .ThenInclude(d => d.BloodGroup)
                .Include(u => u.Donor)
                    .ThenInclude(d => d.TestResults)
                .Include(u => u.Donor)
                    .ThenInclude(d => d.Appointments)
                        .ThenInclude(a => a.Location)
                .FirstOrDefault(u => u.UserId == userId);
        }

    }
}
