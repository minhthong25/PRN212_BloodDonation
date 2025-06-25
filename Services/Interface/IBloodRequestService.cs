using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{   public interface IBloodRequestService
    {
        IEnumerable<BloodRequest> GetAll();
        BloodRequest? GetById(int id);
        void Add(BloodRequest request);
        void Update(BloodRequest request);
        void Delete(int id);
    }
}
