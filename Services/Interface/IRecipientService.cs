using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IRecipientService 
    {
        IEnumerable<Recipient> GetAllRecipients();
        Recipient? GetRecipientById(int id);
        void AddRecipient(Recipient recipient);
        void UpdateRecipient(Recipient recipient);
        void DeleteRecipient(int id);
    }
}
