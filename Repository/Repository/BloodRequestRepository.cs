using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class BloodRequestRepository : IBloodRequestRepository
    {
        private readonly BloodDonationDbContext _context;

        public BloodRequestRepository(BloodDonationDbContext context)
        {
            _context = context;
        }

        public List<BloodRequest> GetAllWithUser()
        {
            return _context.BloodRequests
                           .Include(r => r.Recipient)
                           .ToList();
        }

        public void UpdateStatus(BloodRequest request)
        {
            var item = _context.BloodRequests.FirstOrDefault(r => r.RequestId == request.RequestId);
            if (item != null)
            {
                item.Status = request.Status;
                item.ResponseMessage = request.ResponseMessage;
                _context.SaveChanges();
            }
        }
    }
}