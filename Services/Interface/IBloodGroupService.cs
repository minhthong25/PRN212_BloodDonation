using Repository.Models;
using System.Collections.Generic;

namespace Services.Interface
{
    public interface IBloodGroupService
    {
        List<BloodGroup> GetAllBloodGroups();
        List<BloodGroup> GetAllBloodGroupNames();
    }
} 