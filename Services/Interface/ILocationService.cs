using Repository.Models;

namespace Services.Interface
{
    public interface ILocationService
    {
        List<Location> GetAllLocations();
        Location? GetLocationById(int id);
        void AddLocation(Location location);
        void UpdateLocation(Location location);
        void DeleteLocation(int id);
        List<Location> GetUpcomingEvents();
    }
} 