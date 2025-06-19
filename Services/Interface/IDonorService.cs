using Repository.Models;

namespace Services.Interface
{
    public interface IDonorService
    {
        Donor CreateNewDonor(Donor donor);
        Donor? GetDonorById(int id);
        void AddDonor(Donor donor);
        void UpdateDonor(Donor donor);
        void DeleteDonor(int id);
    }
} 