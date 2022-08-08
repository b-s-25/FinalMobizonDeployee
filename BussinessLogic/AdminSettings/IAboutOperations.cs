using DomainLayer.AdminSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Settings
{
    public interface IAboutOperations
    {
        Task Add(About data);
        Task Edit(About data);
        IEnumerable<About> Get();
    }
}
