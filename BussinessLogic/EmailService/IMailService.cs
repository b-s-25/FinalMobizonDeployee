using DomainLayer.EmailService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic
{
    public interface IMailService
    {
        Task SendEmailAsync(Email email);
    }
}
