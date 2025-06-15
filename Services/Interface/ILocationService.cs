using Repository.Models;

namespace Services.Interface
{
    public interface ILocationService
    {
        Location? GetLocationById(int id);
        void AddLocation(Location location);
        void UpdateLocation(Location location);
        void DeleteLocation(int id);
    }
} 