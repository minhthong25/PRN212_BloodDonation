using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Interface;
using Repository.Models;

namespace Services.Services
{
    public class RecipientsService : IRecipientsService
    {
        private readonly BloodDonationDbContext _context;

        public RecipientsService(BloodDonationDbContext context)
        {
            _context = context;
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
            _context.Recipients.Add(recipient);
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