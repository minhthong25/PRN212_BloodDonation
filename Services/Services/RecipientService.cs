using Repository.Interface;
using Repository.Models;
using Repository.Repository;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Services.Services
{
    public class RecipientService : IRecipientService
    {
        private readonly IGenericRepository<Recipient> _recipientRepository;

        private readonly BloodDonationDbContext _context;

        public RecipientService()
        {
            _recipientRepository = new GenericRepository<Recipient>();
            _context = new BloodDonationDbContext();
        }
        public IEnumerable<Recipient> GetAllRecipients()
        {
            return _context.Recipients.ToList();
        }

        public Recipient? GetRecipientById(int id)
        {
            return _context.Recipients.FirstOrDefault(r => r.RecipientId == id);
        }

        public void AddRecipient(Recipient recipient)
        {
            _recipientRepository.Add(recipient);
            _context.SaveChanges();
        }
        public void UpdateRecipient(Recipient recipient)
        {
            _context.Recipients.Update(recipient);
            _context.SaveChanges();
        }

        public void DeleteRecipient(int id)
        {
            var recipient = _context.Recipients.FirstOrDefault(r => r.RecipientId == id);
            if (recipient != null)
            {
                _context.Recipients.Remove(recipient);
                _context.SaveChanges();
            }
        }
    }
}
