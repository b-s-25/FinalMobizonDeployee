using DomainLayer;
using Microsoft.EntityFrameworkCore;
using Repository;
using RepositoryLayer;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic
{
  public  class MasterDataOperations: GenericRepositoryOperation<MasterData>, IMasterDataOperations
    {
        private readonly IGenericRepositoryOperation<MasterData> _repo;
        private readonly ProductDbContext _dbContext;

        public MasterDataOperations(ProductDbContext dbContext,IGenericRepositoryOperation<MasterData> repo) : base(dbContext)
        {
            _dbContext = dbContext;
            _repo = repo;
        }
        public async Task MasterDataAdd(MasterData data)
        {
            _repo.Add(data);
            _repo.Save();
        }

        public async Task MasterDataDelete(MasterData entity)
        {
              _repo.Delete(entity);
            _repo.Save();
        }

        public IEnumerable<MasterData> GetAllMasterData()
        {
            return _repo.GetAll();
        }

        public async Task MasterDataEdit(MasterData entity)
        {
            _repo.Update(entity);
            _repo.Save();
        }

        public MasterData MasterDataGetById(int Id)
        {
            return _repo.GetById(Id);
        }

     
    }


}
