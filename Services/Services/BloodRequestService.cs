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
        private readonly IGenericRepository<BloodRequest> _bloodRequestRepository;
        private readonly BloodDonationDbContext _context;
        public BloodRequestService() 
        {
            _bloodRequestRepository = new GenericRepository<BloodRequest>();
            _context = new BloodDonationDbContext();
        }
    }
}
