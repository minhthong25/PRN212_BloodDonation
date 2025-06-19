using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Repository.Interface;
using Repository.Models;
using Repository.Repository;
using Services.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Services.Services
{
    public class RequestApprovalService : IRequestApprovalService
    {
        private readonly IGenericRepository<RequestApproval> _requestApprovalRepository;
        private readonly BloodDonationDbContext _context;
        public RequestApprovalService() 
        {
            _requestApprovalRepository = new GenericRepository<RequestApproval>();
            _context = new BloodDonationDbContext();
        }
    }
}
