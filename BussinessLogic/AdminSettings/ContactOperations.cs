using DomainLayer.AdminSettings;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.AdminSettings
{
    public class ContactOperations: IContactOperations
    {
        IGenericRepositoryOperation<AdminContact> _repo;
        public ContactOperations(IGenericRepositoryOperation<AdminContact> repo)
        {
            _repo = repo;
        }
        public async Task Add(AdminContact data)
        {
            _repo.Add(data);
            _repo.Save();

        }

        public async Task Edit(AdminContact data)
        {
            _repo.Update(data);
            _repo.Save();
        }

        public async Task Get(AdminContact data)
        {
            _repo.GetAll();
        }

        public IEnumerable<AdminContact> Get()
        {

            return _repo.GetAll();
        }
    }
}
