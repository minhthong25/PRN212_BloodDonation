using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Interface;
using Repository.Models;
using Repository.Repository;
using Services.Interface;

namespace Services.Services
{
    public class LocationService : ILocationService
    {
        private readonly IGenericRepository<Location>  _locationRepository;

        public LocationService()
        {
            _locationRepository = new GenericRepository<Location>();
        }

        public List<Location> GetAllLocations()
        {
                return _locationRepository.GetAll().ToList(); 
        }

        public Location? GetLocationById(int id)
        {
            try
            {
                return _locationRepository.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving location with ID {id}", ex);
            }
        }

        public void AddLocation(Location location)
        {
            try
            {
                if (location == null)
                    throw new ArgumentNullException(nameof(location));

                _locationRepository.Add(location);
                _locationRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding location", ex);
            }
        }

        public void UpdateLocation(Location location)
        {
            try
            {
                if (location == null)
                    throw new ArgumentNullException(nameof(location));

                _locationRepository.Update(location);
                _locationRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating location", ex);
            }
        }

        public void DeleteLocation(int id)
        {
            try
            {
                var location = _locationRepository.GetById(id);
                if (location != null)
                {
                    _locationRepository.Delete(location);
                    _locationRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting location with ID {id}", ex);
            }
        }

        public List<Location> GetUpcomingEvents()
        {
            return _locationRepository
                .GetAll()
                .Where(l => l.EventDate != null && l.EventDate >= DateTime.Today)
                .OrderBy(l => l.EventDate)
                .ToList();
        }
    }
}
