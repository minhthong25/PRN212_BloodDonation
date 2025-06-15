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
    public class BloodInventoryService : IBloodInventoryService
    {
        private readonly IGenericRepository<BloodInventory> _bloodInventoryRepository;
        private readonly BloodDonationDbContext _context;

        public BloodInventoryService()
        {
            _bloodInventoryRepository = new GenericRepository<BloodInventory>();
            _context = new BloodDonationDbContext();
            
        }

        public List<BloodInventory> GetAllBloodInventory()
        {
            return _context.BloodInventories
                .Include(b => b.BloodGroup)
                .ToList();
        }

        public BloodInventory? UpdateBloodInventory(int bloodGroupId, int quantity)
        {
            try
            {
                var bloodInventory = _bloodInventoryRepository.FirstOrDefault(b => b.BloodGroupId == bloodGroupId);

                if (bloodInventory == null)
                {
                    // Create new inventory entry if it doesn't exist
                    bloodInventory = new BloodInventory
                    {
                        BloodGroupId = bloodGroupId,
                        Quantity = quantity,
                        UpdatedAt = DateTime.Now
                    };
                    _bloodInventoryRepository.Add(bloodInventory);
                }
                else
                {
                    // Update existing inventory
                    bloodInventory.Quantity = quantity;
                    bloodInventory.UpdatedAt = DateTime.Now;
                    _bloodInventoryRepository.Update(bloodInventory);
                }

                _context.SaveChanges();
                return bloodInventory;
            }
            catch (Exception ex)
            {
                // Log the exception here
                return null;
            }
        }
    }
}
