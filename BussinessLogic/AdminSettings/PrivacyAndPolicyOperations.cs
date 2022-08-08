using DomainLayer.AdminSettings;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.AdminSettings
{
    public class PrivacyAndPolicyOperations : IPrivacyAndPolicyOperations
    {
        private readonly IGenericRepositoryOperation<PrivacyAndPolicy> _privacyAndPolicy;
        public PrivacyAndPolicyOperations(IGenericRepositoryOperation<PrivacyAndPolicy> privacyAndPolicy)
        {
            _privacyAndPolicy = privacyAndPolicy;
        }

        public async Task Add(PrivacyAndPolicy data)
        {
            _privacyAndPolicy.Add(data);
            _privacyAndPolicy.Save();
        }

        public async Task Edit(PrivacyAndPolicy data)
        {
            _privacyAndPolicy.Update(data);
            _privacyAndPolicy.Save();
        }

        public IEnumerable<PrivacyAndPolicy> Get()
        {
            return _privacyAndPolicy.GetAll();
        }
    }
}
