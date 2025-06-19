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

        public List<Recipient> GetAllRecipients()
        {
            return _context.Recipients
                .Include(r => r.RecipientNavigation)
                .ToList();
        }

        public Recipient? GetRecipientById(int recipientId)
        {
            return _context.Recipients
                .Include(r => r.RecipientNavigation)
                .FirstOrDefault(r => r.RecipientId == recipientId);
        }

        public void AddRecipient(Recipient recipient)
        {
            _recipientRepository.Add(recipient);
            _context.SaveChanges();
        }
    }
}
