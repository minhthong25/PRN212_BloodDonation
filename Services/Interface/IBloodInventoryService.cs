using Repository.Models;

namespace Services.Interface
{
    public interface IBloodInventoryService
    {
        List<BloodInventory> GetAllBloodInventory();
        BloodInventory? UpdateBloodInventory(int bloodGroupId, int quantity); 
    }
}