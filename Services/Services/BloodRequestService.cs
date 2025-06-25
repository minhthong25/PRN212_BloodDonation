using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class BloodRequestService : IBloodRequestService
    {
        private readonly BloodDonationDbContext _context;

        public BloodRequestService(BloodDonationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<BloodRequest> GetAll()
        {
            return _context.BloodRequests
                .Include(br => br.Recipient)
                .ThenInclude(r => r.RecipientNavigation)
                .Include(br => br.BloodGroup)
                .ToList();
        }

        public BloodRequest? GetById(int id)
        {
            return _context.BloodRequests.FirstOrDefault(r => r.RequestId == id);
        }

        public void Add(BloodRequest request)
        {
            _context.BloodRequests.Add(request);
            _context.SaveChanges();
        }

        public void Update(BloodRequest request)
        {
            _context.BloodRequests.Update(request);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var req = _context.BloodRequests.FirstOrDefault(r => r.RequestId == id);
            if (req != null)
            {
                _context.BloodRequests.Remove(req);
                _context.SaveChanges();
            }
        }
    }
}
