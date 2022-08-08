using DomainLayer;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic
{
  public   interface IUserDataoperation: IGenericRepositoryOperation<Registration>
    {
        IEnumerable<Registration> GetUserData();

    }
}
