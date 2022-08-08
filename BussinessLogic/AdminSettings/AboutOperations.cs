using DomainLayer.AdminSettings;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Settings
{
    public class AboutOperations : IAboutOperations
    {
        private readonly IGenericRepositoryOperation<About> _repo;
        public AboutOperations(IGenericRepositoryOperation<About> repo)
        {
            _repo = repo;
        }

        public async Task Add(About data)
        {
            try
            {
                _repo.Add(data);
                _repo.Save();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task Edit(About data)
        {
            _repo.Update(data);
            _repo.Save();
        }


        public IEnumerable<About> Get()
        {
            return _repo.GetAll();
        }

    }
}
