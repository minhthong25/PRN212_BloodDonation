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
    public class BloodRequestService: IBloodRequestService
    {
        private readonly IBloodRequestRepository _repository;

        public BloodRequestService()
        {
            _repository = new BloodRequestRepository(new BloodDonationDbContext());
        }

        public List<BloodRequest> GetAllRequestsWithUser()
        {
            return _repository.GetAllWithUser();
        }

        public void UpdateRequestStatus(BloodRequest request)
        {
            _repository.UpdateStatus(request);
        }
    }
}
