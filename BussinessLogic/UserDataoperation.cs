using RepositoryLayer.Interface;
using RepositoryLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer;
using Repository;

namespace BussinessLogic
{
   public  class UserDataoperation:GenericRepositoryOperation<Registration>,IUserDataoperation
    {
        private readonly IGenericRepositoryOperation<Registration> _repo;
        private readonly ProductDbContext _dbContext;
        public UserDataoperation( ProductDbContext dbContext, IGenericRepositoryOperation<Registration> repo):base(dbContext)
        {
            _dbContext = dbContext;
            _repo = repo;

        }

        public IEnumerable<Registration> GetUserData()
        {
            return _repo.GetAll();
        }

        
    }
}
