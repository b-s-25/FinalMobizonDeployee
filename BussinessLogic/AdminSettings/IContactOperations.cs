using DomainLayer.AdminSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.AdminSettings
{
    public interface IContactOperations
    {
        Task Add(AdminContact data);
        Task Edit(AdminContact data);
        IEnumerable<AdminContact> Get();
    }
}
