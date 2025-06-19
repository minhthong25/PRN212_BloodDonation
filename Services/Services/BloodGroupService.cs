using Repository.Interface;
using Repository.Models;
using Repository.Repository;
using Services.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Services.Services
{
    public class BloodGroupService : IBloodGroupService
    {
        private readonly IGenericRepository<BloodGroup> _bloodGroupRepository;
        private readonly BloodDonationDbContext _context;
        public BloodGroupService()
        {
            _bloodGroupRepository = new GenericRepository<BloodGroup>();
            _context = new BloodDonationDbContext();
        }
        public List<BloodGroup> GetAllBloodGroups()
        {
            return _context.BloodGroups.ToList();
        }
        public List<BloodGroup> GetAllBloodGroupNames()
        {
            return _context.BloodGroups
                .Select(bg => new BloodGroup { BloodGroupId = bg.BloodGroupId, GroupName = bg.GroupName })
                .ToList();
        }
    }
} 