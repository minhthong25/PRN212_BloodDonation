using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IRecipientService 
    {
        List<Repository.Models.Recipient> GetAllRecipients();
        Repository.Models.Recipient? GetRecipientById(int recipientId);

        void AddRecipient(Repository.Models.Recipient recipient);
    }
}
