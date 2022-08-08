using DomainLayer.AdminSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.AdminSettings
{
    public interface IPrivacyAndPolicyOperations
    {
        Task Add(PrivacyAndPolicy data);
        Task Edit(PrivacyAndPolicy data);
        IEnumerable<PrivacyAndPolicy> Get();
    }
}
