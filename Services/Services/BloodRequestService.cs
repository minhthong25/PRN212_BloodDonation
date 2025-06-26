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
    public class BloodRequestService : IBloodRequestService
    {
        private readonly IBloodRequestRepository _repository;
        private readonly BloodDonationDbContext _context;

        public BloodRequestService()
        {
            _repository = new BloodRequestRepository(new BloodDonationDbContext());
            _context = new BloodDonationDbContext();
        }

        public List<BloodRequest> GetAllRequestsWithUser()
        {
            return _repository.GetAllWithUser();
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
        public void UpdateRequestStatus(BloodRequest request)
        {
            _repository.UpdateStatus(request);
        }
    }
}