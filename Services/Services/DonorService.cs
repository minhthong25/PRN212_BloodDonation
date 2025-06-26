using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Repository.Models;
using Repository.Repository;
using Services.Interface;

namespace Services.Services
{
    public class DonorService : IDonorService
    {
        private readonly IGenericRepository<Donor> _donorRepository;
        public DonorService()
        {
            _donorRepository = new GenericRepository<Donor>();
        }

        public Donor CreateNewDonor(Donor donor)
        {
            try
            {
                _donorRepository.Add(donor);
                _donorRepository.SaveChanges();
                return donor;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating new donor", ex);
            }
        }

        public Donor? GetDonorById(int id)
        {
            try
            {
                return _donorRepository.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving donor with ID {id}", ex);
            }
        }

        public void AddDonor(Donor donor)
        {
            try
            {
                if (donor == null)
                    throw new ArgumentNullException(nameof(donor));

                _donorRepository.Add(donor);
                _donorRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding donor", ex);
            }
        }

        public void UpdateDonor(Donor donor)
        {
            try
            {
                if (donor == null)
                    throw new ArgumentNullException(nameof(donor));

                _donorRepository.Update(donor);
                _donorRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating donor", ex);
            }
        }

        public void DeleteDonor(int id)
        {
            try
            {
                var donor = _donorRepository.GetById(id);
                if (donor != null)
                {
                    _donorRepository.Delete(donor);
                    _donorRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting donor with ID {id}", ex);
            }
        }

        public Donor GetDonorByUserId(int userId)
        {
            var donor = _donorRepository.Query()
                .Include(d => d.DonorNavigation)
                .FirstOrDefault(d => d.DonorNavigation.UserId == userId);

            if (donor == null)
            {
                throw new Exception($"No donor found for User ID {userId}");
            }

            return donor;
        }
    }
}
